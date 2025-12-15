using LexiGeht.ViewModels.Main;

namespace LexiGeht.Views.Main;

public partial class QuestionPage : ContentPage
{
	public QuestionPage(QuestionViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }
}