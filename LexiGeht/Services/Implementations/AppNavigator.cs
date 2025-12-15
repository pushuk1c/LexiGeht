using LexiGeht.Services.Interfaces;
using LexiGeht.Views.Auth;
using LexiGeht.Views.Main;

namespace LexiGeht.Services.Implementations
{
    public class AppNavigator : IAppNavigator
    {
        private readonly IServiceProvider _serviceProvider;

        public AppNavigator(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public void SwitchToShell()
        {
            var shell = _serviceProvider.GetRequiredService<AppShell>();
                     

            Application.Current.MainPage = shell;

           

        }

        public void SwitchToLogin()
        {
            var loginPage = _serviceProvider.GetRequiredService<LoginPage>();
            Application.Current.MainPage = new NavigationPage(loginPage);
        }
    }
}
