using LexiGeht.ViewModels.Main;

namespace LexiGeht.Views.Main;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }
}