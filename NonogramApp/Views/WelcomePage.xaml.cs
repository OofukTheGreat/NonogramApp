using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class WelcomePage : ContentPage
{
	public WelcomePage(WelcomeViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}