

namespace LexiGeht.Services.Interfaces
{
    public interface IDialogService
    {
        Task AlertAsync(string title, string message, string ok = "OK");
    }
}
