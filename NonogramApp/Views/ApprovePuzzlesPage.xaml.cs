using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class ApprovePuzzlesPage : ContentPage
{
	public ApprovePuzzlesPage(ApprovePuzzlesViewModel vm)
	{
		InitializeComponent();
        this.BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (this.BindingContext is ApprovePuzzlesViewModel)
        {
            ((ApprovePuzzlesViewModel)this.BindingContext).InitData();
        }
    }
}