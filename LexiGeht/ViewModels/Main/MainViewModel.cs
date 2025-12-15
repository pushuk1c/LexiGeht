using LexiGeht.Models;
using LexiGeht.Repositories.Interfaces;
using LexiGeht.Services.Interfaces;
using LexiGeht.Services.Media;
using LexiGeht.Views.Main;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LexiGeht.ViewModels.Main
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IQuizService _quizService;
        private readonly IMediaStoreService _mediaService;
                
        public ObservableCollection<Cours> Courses { get; set; }
        public ObservableCollection<Quiz> Quizzes { get; set; }

        public ICommand OpenQuizCommand { get; }

        public ICommand TabSelectedCommand { get; }

        public MainViewModel(INavigationService navigationService, IQuizService quizService, IMediaStoreService mediaStoreService)
        {
            _navigationService = navigationService;
            _mediaService = mediaStoreService;
            _quizService = quizService;

            OpenQuizCommand = new Command<Quiz>(async q => await OpenQuizAsync(q));
            
            Courses = new ObservableCollection<Cours>
            {
                new Cours
                {
                    Id = 1,
                    Title = "Deutsch A1",
                    Description = "Learn the basics of Spanish language.",
                    Image = "A1.png"
                },
                new Cours
                {
                    Id = 2,
                    Title = "Deutsch A2",
                    Description = "Start your journey in learning French.",
                    Image = "A2.png"
                },
                new Cours
                {
                    Id = 3,
                    Title = "Deutsch B1",
                    Description = "Get to know the essentials of German.",
                    Image = "B1.png"
                },
                new Cours
                {
                    Id = 4,
                    Title = "Deutsch B2",
                    Description = "Get to know the essentials of German.",
                    Image = "B2.png"
                }
            };

            Quizzes = new ObservableCollection<Quiz>();

            Init();
        }

        public void Init()
        {
            LoadQuizzesAsync();
        }

        private async void LoadQuizzesAsync()
        {
            var quizzes = await _quizService.GetAllQuizzesAsync();

            foreach (var quiz in quizzes)
            {
                quiz.ImagePathSource = await _mediaService.GetImageSourceAsync(MediaFolder.QuizzesImages, quiz.ImagePath);
                quiz.Progress = 0.75; 
                Quizzes.Add(quiz);
            }
        }

        private async Task OpenQuizAsync(object obj)
        {
            if (obj is Quiz selectedQuiz)
            {
                _navigationService.PushAsync<QuizPage>(selectedQuiz);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
