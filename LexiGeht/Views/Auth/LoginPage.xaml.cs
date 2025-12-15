using LexiGeht.ViewModels.Auth;
namespace LexiGeht.Views.Auth;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();

        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = vm;
    }
}