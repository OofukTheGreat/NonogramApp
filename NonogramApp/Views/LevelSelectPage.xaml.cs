using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class LevelSelectPage : ContentPage
{
	public LevelSelectPage(LevelSelectViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}