using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}