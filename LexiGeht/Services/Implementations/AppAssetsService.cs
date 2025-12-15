

namespace LexiGeht.Services.Interfaces
{
    public class AppAssetsService : IAppAssetsService
    {
        public async Task<Stream> OpenAsync(string fileName, CancellationToken ct = default)
            => await FileSystem.OpenAppPackageFileAsync(fileName);

        public async Task<string> EnsureLocalAsync(string fileName, string? relativTargetFolder = null,
                                            bool overwrite = false, CancellationToken ct = default)
        {
            var targetFolder = FileSystem.AppDataDirectory;
            if (!string.IsNullOrWhiteSpace(relativTargetFolder))
            {
                targetFolder = Path.Combine(targetFolder, relativTargetFolder);
                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);
            }
            
            var targetFile = Path.Combine(targetFolder, fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(targetFile)!);

            if (overwrite || !File.Exists(targetFile) || new FileInfo(targetFile).Length == 0)
            {
                await using var sourceStream = await OpenAsync(fileName, ct);
                await using var targetStream = File.Open(targetFile, FileMode.Create, FileAccess.Write);
                await sourceStream.CopyToAsync(targetStream, ct);
            }
            return targetFile;
        }

    }
}
