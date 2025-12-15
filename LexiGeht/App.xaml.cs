using LexiGeht.Services.Interfaces;
using LexiGeht.Views.Auth;

namespace LexiGeht
{
    public partial class App : Application
    {
        private readonly IUserService _userService;
        private readonly IAppNavigator _appNavigator;


        public static IServiceProvider _serviceProviderStatic;

        public App(IUserService userService, IAppNavigator appNavigator, IServiceProvider serviceProvider)
        {
            _userService = userService;
            _appNavigator = appNavigator;
                     
            _serviceProviderStatic = serviceProvider;

            InitializeComponent();

            MainPage = new ContentPage
            {
                Content = new ActivityIndicator
                {
                    IsRunning = true,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                }
            };

            SetMainPageAsync();
        }

        private async void SetMainPageAsync()
        {
            var currentUserResult = await _userService.GetCurrentUserAsync();
            if (currentUserResult.IsSuccess && currentUserResult.Data != null)
            {
                _appNavigator.SwitchToShell();
            }
            else
            {
                _appNavigator.SwitchToLogin();
            }
        }
              
    }
}