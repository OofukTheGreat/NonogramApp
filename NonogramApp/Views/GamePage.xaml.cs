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
		LeaderboardViewModel vm = new LeaderboardViewModel();
        var popup = new Leaderboard(vm);
        this.ShowPopup(popup);
    }
}