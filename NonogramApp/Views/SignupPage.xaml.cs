using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class SignupPage : ContentPage
{
	public SignupPage(SignupPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}