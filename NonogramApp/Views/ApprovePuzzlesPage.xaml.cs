using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class ApprovePuzzlesPage : ContentPage
{
	public ApprovePuzzlesPage(ApprovePuzzlesViewModel vm)
	{
		InitializeComponent();
        this.BindingContext = vm;
    }
}