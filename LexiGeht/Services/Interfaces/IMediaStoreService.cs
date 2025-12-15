

using LexiGeht.Services.Media;

namespace LexiGeht.Services.Interfaces
{
    public interface IMediaStoreService
    {
        Task<ImageSource> GetImageSourceAsync(MediaFolder folder, string imagePath, string? remoteUrl = null, bool overWrite = false, CancellationToken ct = default); 
    
        Task<string> GetAudioFileAsync(MediaFolder folder, string audioPath, string? remoteUrl = null, bool overWrite = false, CancellationToken ct = default);
    }
}
