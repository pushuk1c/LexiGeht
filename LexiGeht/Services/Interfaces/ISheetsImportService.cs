using LexiGeht.Common;

namespace LexiGeht.Services.Interfaces
{
    public interface ISheetsImportService
    {
        Task<OperationResult<bool>> ImportAsync();
    }
}
