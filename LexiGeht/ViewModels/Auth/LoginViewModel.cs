using LexiGeht.Services.Interfaces;
using LexiGeht.Views.Auth;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LexiGeht.ViewModels.Auth
{
    public class LoginViewModel: INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IAppNavigator _appNavigator;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoginCommand { get; }

        public ICommand SignUpCommand { get; }

        public LoginViewModel(IAuthService authService, INavigationService navigationService, 
                    IDialogService dialogService, IAppNavigator appNavigator)
        {
            _authService = authService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _appNavigator = appNavigator;

            LoginCommand = new Command(OnLogin);
            SignUpCommand = new Command(OnSignUp);
        }

        private async void OnLogin()
        {
            var result = await _authService.LoginAsync(Username, Password);
            if (result.IsSuccess)
            {
                // Navigate to the main application page
                _appNavigator.SwitchToShell();
            }
            else
            {
                // Show error message
                await _dialogService.AlertAsync("Login Failed", result.ErrorMessage ?? "An error occurred during login.", "OK");
                
            }
        }

        private async void OnSignUp()
        {
            await _navigationService.PushAsync<SignupPage>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
