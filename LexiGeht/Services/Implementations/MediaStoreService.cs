using LexiGeht.Services.Interfaces;
using LexiGeht.Services.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace LexiGeht.Services.Implementations
{
    public class MediaStoreService : IMediaStoreService
    {
        private readonly HttpClient _httpClient;

        private readonly string remoteGoogleUrl = $"https://script.google.com/macros/s/AKfycbysvASSS_PNyxG5n_IVdrj0230PMaTC6ugxPXN7Dz7L_KMpZ6KvEkTPajI7yJhTF8QZdg/exec";

        private readonly SemaphoreSlim _dlLock = new (1, 1);

        public MediaStoreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAudioFileAsync(MediaFolder folder, string audioPath, string? remoteUrl = null, bool overWrite = false, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(remoteUrl))
                remoteUrl = remoteGoogleUrl;

            var localPath = await EnsureDownloadedFile(
                subFolder: folder, rootFolder: "audios",
                fileName: audioPath, remoteUrl: remoteUrl, 
                overWrite: overWrite, ct: ct);

            if (localPath != null)
                return localPath;

            return audioPath;
        }

        public async Task<ImageSource> GetImageSourceAsync(MediaFolder folder, string imagePath, string? remoteUrl = null, bool overWrite = false, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(remoteUrl))
                remoteUrl = remoteGoogleUrl;

            var localPath = await EnsureDownloadedFile(
                subFolder: folder, rootFolder: "images",
                fileName: imagePath, remoteUrl: remoteUrl, 
                overWrite: overWrite, ct: ct);

            if (localPath != null)
                return ImageSource.FromFile(localPath);

            return ImageSource.FromFile(imagePath);

        }

        record ProxyMeta(string? id, string? url, string? name);

        private async Task<string> EnsureDownloadedFile(MediaFolder subFolder, string rootFolder, string fileName, string? remoteUrl = null, bool overWrite = false, CancellationToken ct = default)
        {
            var folder = Path.Combine(FileSystem.AppDataDirectory, "media", rootFolder, subFolder.ToKey());
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var name = NormalizeFileName(fileName, remoteUrl);
            var localPath = Path.Combine(folder, name);

            if (System.IO.File.Exists(localPath))
            {
                if (overWrite)
                {
                    System.IO.File.Delete(localPath);
                }
                else
                {
                    return localPath;
                }
            }  

            if (string.IsNullOrEmpty(remoteUrl))
                return null;

            try
            {
                remoteUrl = NormalizeGoogleDriveUrl(remoteUrl, subFolder.ToKey(), fileName);

                await _dlLock.WaitAsync(ct);
                if (System.IO.File.Exists(localPath))
                    return localPath;

                var metaResp = await _httpClient.GetAsync(remoteUrl, ct);
                metaResp.EnsureSuccessStatusCode();

                var metastr = await metaResp.Content.ReadAsStringAsync(ct);
                var meta = System.Text.Json.JsonSerializer.Deserialize<ProxyMeta>(metastr);
                if (meta == null || string.IsNullOrEmpty(meta.id))
                    throw new InvalidOperationException("Invalid proxy metadata.");

                await DownloadFileFromGoogleDriveAsync(meta.id, localPath, ct);

                return localPath;

            } catch (Exception e)
            { 
                return null;

            } finally
            {
                if (_dlLock.CurrentCount == 0)
                    _dlLock.Release();
            }

        }

        private static string NormalizeFileName(string fileName, string? remoteUrl)
        {
            if (Path.HasExtension(fileName))
                return Path.GetFileName(fileName);

            try
            {
                if (!string.IsNullOrEmpty(remoteUrl))
                {
                    var uri = new Uri(remoteUrl);
                    var last = Path.GetFileName(uri.LocalPath);
                    return $"{Path.GetFileNameWithoutExtension(fileName)}{Path.GetExtension(last)}";
                }
            }
            catch { /* ignore */}
            
            return Path.GetFileName(fileName);
        }

        private static string NormalizeGoogleDriveUrl(string url, string folderKey, string fileName)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            var sb = new StringBuilder();
            sb.Append(url);
            sb.Append(url.Contains('?') ? '&' : '?');
            sb.Append("folder=").Append(Uri.EscapeDataString(folderKey));
            sb.Append("&name=").Append(Uri.EscapeDataString(fileName));
            sb.Append("&token=jU7krR9rcQxJ2qzY6xpFiu4leP1zdO");

            return sb.ToString();
        }
        
        private static async Task DownloadFileFromGoogleDriveAsync(string fileId, string localPath, CancellationToken ct)
        {
            var baseUrl = "https://drive.google.com/uc?export=download";
            var cookieContainer = new CookieContainer();
            using var handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.All
            };

            using var httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

            using var initialResponse = await httpClient.GetAsync($"{baseUrl}&id={Uri.UnescapeDataString(fileId)}", HttpCompletionOption.ResponseHeadersRead, ct);
            initialResponse.EnsureSuccessStatusCode();

            var mt1 = initialResponse.Content.Headers.ContentType?.MediaType;

            if (IsBinary(mt1))
            {
                await SaveFileFromGoogleDriveAsync(initialResponse, localPath, ct);
                return;
            }

            // find confirm token
            var html = await initialResponse.Content.ReadAsStringAsync(ct);

            var m =
                Regex.Match(html, @"confirm=([0-9A-Za-z_]+)&") 
                ?? Regex.Match(html, @"name=""confirm""\s+value=""([0-9A-Za-z_]+)""")
                ?? Regex.Match(html, @"download_warning[^&]*&confirm=([0-9A-Za-z_]+)");

            if (m.Success)
            {
                var confirm = m.Groups[1].Value;
                var url2 = $"{baseUrl}&confirm={confirm}&id={Uri.EscapeDataString(fileId)}";

                using var response = await httpClient.GetAsync(url2, HttpCompletionOption.ResponseHeadersRead, ct);
                response.EnsureSuccessStatusCode();

                var mt2 = response.Content.Headers.ContentType?.MediaType;
                if (!IsBinary(mt2))
                {
                    var head = await response.Content.ReadAsStringAsync(ct);
                    throw new InvalidOperationException($"Drive still returned HTML: {mt2}. First 200: {head[..Math.Min(200, head.Length)]}");
                }

                await SaveFileFromGoogleDriveAsync(initialResponse, localPath, ct);
                return;
            }

            throw new InvalidOperationException($"Google Drive returned HTML instead of file. Can't find confirm token. First 200: {html[..Math.Min(200, html.Length)]}");

        }

        private static bool IsBinary(string? mt)
        {
            return !string.IsNullOrEmpty(mt) 
                    && (mt.StartsWith("image/", StringComparison.OrdinalIgnoreCase) 
                    || mt.StartsWith("audio/", StringComparison.OrdinalIgnoreCase) 
                    || string.Equals(mt, "application/octet-stream", StringComparison.OrdinalIgnoreCase));
        }

        private static async Task SaveFileFromGoogleDriveAsync(HttpResponseMessage resp, string localPath, CancellationToken ct)
        {            
            await using var contentStream = await resp.Content.ReadAsStreamAsync(ct);
            await using var fileStream = System.IO.File.Create(localPath);
            await contentStream.CopyToAsync(fileStream, ct);
        }

    }
}
