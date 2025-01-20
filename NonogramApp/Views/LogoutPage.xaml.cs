using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class LogoutPage : ContentPage
{
	public LogoutPage(LogoutPageViewModel vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}
}