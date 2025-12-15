using LexiGeht.Models;
using LexiGeht.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LexiGeht.ViewModels.Main
{
    public class ProfileViewModel: INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;
        private readonly IAppNavigator _appNavigator;

        private int userId;

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

        public ICommand SaveCommand { get; }
        public ICommand SignOutCommand { get; } 

        public ProfileViewModel(IUserService userService, IDialogService dialogService, IAppNavigator appNavigator)
        {            
            _userService = userService;
            _dialogService = dialogService;
            _appNavigator = appNavigator;

            InitializeData();

            SaveCommand = new Command(SaveProfile);
            SignOutCommand = new Command(SignOut);

        }

        private void InitializeData()
        {
            var currentUserResult = Task.Run(() => _userService.GetCurrentUserAsync()).Result;
            if (currentUserResult.IsSuccess)
            {
                userId = currentUserResult.Data.Id;
                Username = currentUserResult.Data.Username;
                Email = currentUserResult.Data.Email;
            }
            else
            {
                _dialogService.AlertAsync("Error", "Failed to load user data.", "OK");
            }
        }

        public async void SaveProfile()
        {            
            var result = await _userService.UpdateUserAsync(userId, Username, Email, Password);
            if (result.IsSuccess)
            {
                await _dialogService.AlertAsync("Success", "Profile updated successfully.", "OK");
            }
            else
            {
                await _dialogService.AlertAsync("Error", "Failed to update profile: " + result.ErrorMessage, "OK");
            }
        }

        public async void SignOut()
        {
            var result = await _userService.LogOut();
            if (result.IsSuccess)
            {
                _appNavigator.SwitchToLogin();
            }
            else
            {
                await _dialogService.AlertAsync("Error", "Failed to sign out: " + result.ErrorMessage, "OK");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
