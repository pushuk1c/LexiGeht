using LexiGeht.Services.Interfaces;
using LexiGeht.Views.Auth;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LexiGeht.ViewModels.Auth
{
    public class SignupViewModel : INotifyPropertyChanged
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

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
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
               
        public ICommand SignUpCommand { get; }

        public SignupViewModel(IAuthService authService, INavigationService navigationService, 
                    IDialogService dialogService, IAppNavigator appNavigator)
        {
            _authService = authService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _appNavigator = appNavigator;

            SignUpCommand = new Command(OnSignUp);
        }
        
        private async void OnSignUp()
        {
            var result = await _authService.RegisterAsync(Username, Email, Password);
            if (result.IsSuccess) 
            {
                await _dialogService.AlertAsync("Signup successful!", "Success", "OK");
                _appNavigator.SwitchToShell();
            }
            else
            {
                await _dialogService.AlertAsync(result.ErrorMessage ?? "Signup failed. Please try again.", "Error", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
