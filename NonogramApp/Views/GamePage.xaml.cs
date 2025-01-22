using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class GamePage : ContentPage
{
	public GamePage(GameViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}