using LexiGeht.Services.Interfaces;
using LexiGeht.ViewModels.Main;

namespace LexiGeht.Views.Main;

public partial class QuizResultPage : ContentPage, IQueryAttributable
{
	public QuizResultPage(QuizResultViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if(BindingContext is QuizResultViewModel vm && query.TryGetValue("Data", out var obj))
            vm.Apply(obj);
    }
}