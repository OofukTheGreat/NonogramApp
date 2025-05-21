using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (this.BindingContext is ProfileViewModel)
        {
            ((ProfileViewModel)this.BindingContext).InitData();
        }
    }
}