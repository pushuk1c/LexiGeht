
namespace LexiGeht.Services.Interfaces
{
    public interface IAppAssetsService
    {
        Task<Stream> OpenAsync(string fileName, CancellationToken ct = default);

        Task<string> EnsureLocalAsync(string fileName,
            string? relativTargetFolder = null,
            bool overwrite = false,
            CancellationToken ct = default);
    }
}
