using CommunityToolkit.Maui.Views;
using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class GamePage : ContentPage
{
	public GamePage(GameViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
	public void DisplayPopup()
	{
        var popup = new SimplePopup();
        this.ShowPopup(popup);
    }
}