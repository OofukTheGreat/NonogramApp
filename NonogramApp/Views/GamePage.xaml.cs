using CommunityToolkit.Maui.Views;
using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class GamePage : ContentPage
{
	private readonly IServiceProvider serviceProvider;
	public GamePage(GameViewModel vm, IServiceProvider sp)
	{
		this.serviceProvider = sp;
		this.BindingContext = vm;
		vm.OpenPopup += DisplayPopup;
		InitializeComponent();
	}
	public void DisplayPopup(List<string> l)
	{
        var popup = new Leaderboard((GameViewModel)this.BindingContext);
        this.ShowPopup(popup);
    }
}