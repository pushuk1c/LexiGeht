using LexiGeht.Models;
using LexiGeht.Services.Interfaces;
using LexiGeht.Views.Main;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace LexiGeht.ViewModels.Main
{
    public class QuizResultViewModel : INotifyPropertyChanged, IAcceptParameter
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
       
        public int Total
        {
            get => _total;
            private set
            {
                if (_total == value) return;
                _total = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Progress));
            }
        }
        public int Score
        {
            get => _score;
            private set
            {
                if (_score == value) return;
                _score = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Progress));
            }
        }

        public float Progress
        {
            get => Total == 0 ? 0 : (float)Score / Total;
        }    

        private int _total = 0;
        private int _score = 0;

        public ICommand EndQuizCommand { get; }

        public QuizResultViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;   

            EndQuizCommand = new Command(async () => await EndQuiz());
        }
        
        private async Task EndQuiz()
        {
            await _navigationService.PushAsync<MainPage>();
        }

        public void Apply(object? parameter)
        {
            if (parameter is (int total, int score))
            {
                Total = total;
                Score = score;
            }
            else if (parameter is int value)
            {
                Total = value;
                Score = value;
            }
            else
            {
                Total = 0;
                Score = 0;
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
