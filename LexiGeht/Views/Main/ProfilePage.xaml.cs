using LexiGeht.ViewModels.Main;

namespace LexiGeht.Views.Main;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }
}