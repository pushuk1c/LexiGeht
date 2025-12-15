using LexiGeht.Helpers;
using LexiGeht.Models;
using LexiGeht.Services.Interfaces;
using LexiGeht.Services.Media;
using LexiGeht.ViewModels.Main.QuestionParts;
using LexiGeht.Views.Main;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LexiGeht.ViewModels.Main
{
    public class QuestionViewModel : INotifyPropertyChanged
    {
        private readonly IQuizRunnerService _quizRunnerService;
        private readonly INavigationService _navigationService;
        private readonly IMediaStoreService _mediaStoreService;
        private readonly IDialogService _dialogService;

        public QuestionViewModel(IQuizRunnerService quizRunnerService, INavigationService navigationService,
                IDialogService dialogService, IMediaStoreService mediaStoreService)
        {
            _quizRunnerService = quizRunnerService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _mediaStoreService = mediaStoreService;

            SubmitAnswerCommand = new Command(OnSubmitAnswer);

            RefreshQuestion();
        }

        private Question? _currentQuestion;
        public Question? CurrentQuestion
        {
            get => _currentQuestion;
            private set
            {
                if (_currentQuestion == value) return;
                _currentQuestion = value;
                OnPropertyChanged();
            }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked == value) return;
                _isChecked = value;
                OnPropertyChanged();
            }
        }

        public int Index => _quizRunnerService.Index;
        public int Total => _quizRunnerService.Total;
        public int Score => _quizRunnerService.Score;
        public bool IsVisibleSubmit => CurrentQuestion?.CorrectAnswers.Count > 1;

        public ObservableCollection<AnswerChoice> Answers { get; } = new();
        public ObservableCollection<int> SelectedAnswerIds { get; set; } = new ObservableCollection<int>();

        public ICommand ToggleAnswerCommand => new Command<int>(Id =>
        {
            if (IsChecked) 
                return;

            if (SelectedAnswerIds.Contains(Id))
                SelectedAnswerIds.Remove(Id);
            else
                SelectedAnswerIds.Add(Id);

            foreach(var answer in Answers)
                answer.State = SelectedAnswerIds.Contains(answer.Id) ? AnswerState.Selected : AnswerState.None;   
           
            if(!IsVisibleSubmit)
                OnSubmitAnswer();

        });

        public ICommand SubmitAnswerCommand { get; } 
        public ICommand NextCommand => new Command(() =>
        {
            _quizRunnerService.Next();
            if (_quizRunnerService.IsFinished)
            {
                OpenResults();

                return;
            }

            RefreshQuestion();

        });

        private async void OnSubmitAnswer()
        {
            if (SelectedAnswerIds.Count == 0 || IsChecked)
                return;

            _quizRunnerService.SubmitAnswer(SelectedAnswerIds.ToList());
            OnPropertyChanged(nameof(Score));

            var correctIds = CurrentQuestion?.CorrectAnswers
                .Select(a => a.Id)
                .ToHashSet() ?? new HashSet<int>();

            foreach (var answer in Answers)
            {
                if (correctIds.Contains(answer.Id))
                    answer.State = AnswerState.Correct;
                else if (SelectedAnswerIds.Contains(answer.Id))
                    answer.State = AnswerState.InCorrect;
                else
                    answer.State = AnswerState.None;
            }

            IsChecked = true;

        }

        private async void RefreshQuestion()
        {
            IsChecked = false;

            CurrentQuestion = _quizRunnerService.CurrentQuestion;
            
            Answers.Clear();
            SelectedAnswerIds.Clear();
            
            foreach(var answer in CurrentQuestion?.Answers ?? Enumerable.Empty<Answer>())
            {
                var answerChoice = new AnswerChoice
                {
                    Id = answer.Id,
                    Title = answer.Title,
                    State = AnswerState.None
                };
                Answers.Add(answerChoice);
            }

            if (CurrentQuestion != null)
                CurrentQuestion.ImagePathSource = await _mediaStoreService.GetImageSourceAsync(MediaFolder.QuestionsImages, CurrentQuestion.ImagePath);

            Answers.Shuffle();

            OnPropertyChanged(nameof(CurrentQuestion));
            OnPropertyChanged(nameof(Index));
            OnPropertyChanged(nameof(Total));
        }

        public async Task OpenResults()
        {
            await _navigationService.PushAsync<QuizResultPage>((Total, Score));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
