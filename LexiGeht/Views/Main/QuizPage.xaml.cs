using LexiGeht.Services.Interfaces;
using LexiGeht.ViewModels.Main;

namespace LexiGeht.Views.Main;

public partial class QuizPage : ContentPage, IQueryAttributable
{
	public QuizPage(QuizViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if(BindingContext is QuizViewModel vm && query.TryGetValue("Data", out var obj))
            vm.Apply(obj);
    }
}