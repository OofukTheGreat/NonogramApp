using NonogramApp.ViewModels;

namespace NonogramApp.Views;

public partial class CreateLevelsPage : ContentPage
{
	public CreateLevelsPage(CreateLevelsViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}