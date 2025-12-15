using LexiGeht.ViewModels.Auth;

namespace LexiGeht.Views.Auth;

public partial class SignupPage : ContentPage
{
	public SignupPage(SignupViewModel vm)
	{
		InitializeComponent();

        NavigationPage.SetHasNavigationBar(this, false);

		BindingContext = vm;
    }
}