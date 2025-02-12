using CommunityToolkit.Maui.Views;
using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class GamePage : ContentPage
{
	public GamePage(GameViewModel vm)
	{
		this.BindingContext = vm;
		vm.OpenPopup += DisplayPopup;
		InitializeComponent();
	}
	public void DisplayPopup(List<string> l)
	{
        var popup = new Leaderboard();
        this.ShowPopup(popup);
    }
}