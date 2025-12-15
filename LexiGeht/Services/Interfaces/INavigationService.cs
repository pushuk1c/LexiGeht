

namespace LexiGeht.Services.Interfaces
{
    public interface INavigationService
    {
        Task PushAsync<TPage>(object? parameter = null) where TPage : Page;
        Task PopAsync();
        Task GoToRootAsync();

    }
}
