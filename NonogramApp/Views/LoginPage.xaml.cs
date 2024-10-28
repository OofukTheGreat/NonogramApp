using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}