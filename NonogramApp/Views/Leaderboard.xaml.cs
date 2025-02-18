using CommunityToolkit.Maui.Views;
using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class Leaderboard : Popup
{
    public Leaderboard(GameViewModel vm)
    {
        this.BindingContext = vm;
        InitializeComponent();
    }


}