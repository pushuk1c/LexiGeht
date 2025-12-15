using LexiGeht.Views.Main;

namespace LexiGeht
{
    public partial class AppShell : Shell
    {
       
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(QuizPage), typeof(QuizPage));
            Routing.RegisterRoute(nameof(QuestionPage), typeof(QuestionPage));
            Routing.RegisterRoute(nameof(QuizResultPage), typeof(QuizResultPage));  
        }
    }
}
