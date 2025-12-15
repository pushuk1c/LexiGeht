using LexiGeht.ViewModels.Main;

namespace LexiGeht.Views.Main
{
    public partial class MainPage : ContentPage
    {
       
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as MainViewModel)?.Init();
        }


    }
}
