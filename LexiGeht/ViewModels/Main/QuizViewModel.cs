using LexiGeht.Models;
using LexiGeht.Services.Interfaces;
using LexiGeht.Views.Main;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace LexiGeht.ViewModels.Main
{
    public class QuizViewModel : INotifyPropertyChanged, IAcceptParameter
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IQuizAgregateService _quizAgregateService;
        private readonly IQuizRunnerService _quizRunnerService;
       
        private Quiz? _quiz;
        public Quiz? Quiz
        {
            get => _quiz;
            private set
            {
                if (_quiz == value) return;
                _quiz = value;
                OnPropertyChanged(); 
            }
        }

        public ICommand StartQuizCommand { get; }

        public QuizViewModel(IQuizAgregateService quizAgregateService, IQuizRunnerService quizRunnerService, 
                            INavigationService navigationService, IDialogService dialogService)
        {
            _quizAgregateService = quizAgregateService;
            _quizRunnerService = quizRunnerService;

            _dialogService = dialogService;
            _navigationService = navigationService;           

            StartQuizCommand = new Command(async () => await StartQuizAsync(), () => Quiz != null);
        }

        private async Task StartQuizAsync()
        {
            var result = await _quizAgregateService.GetAsync(Quiz!.Id);
            if (!result.IsSuccess)
            {
                await _dialogService.AlertAsync("Error", result.ErrorMessage ?? "Unknown error occurred.", "OK");
                return;
            }

            var quizAgregate = result.Data;
            if (quizAgregate == null || !quizAgregate.Questions.Any())
            {
                await _dialogService.AlertAsync("No Questions", "This quiz has no questions available.", "OK");
                return;
            }

            _quizRunnerService.StartQuiz(quizAgregate.Questions);

            await _navigationService.PushAsync<QuestionPage>();

        }

        public void Apply(object? parameter)
        {
            Quiz = Quiz as Quiz ?? parameter as Quiz;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
