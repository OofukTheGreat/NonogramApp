using NonogramApp.ViewModels;
using NonogramApp.Views;

namespace NonogramApp
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel vm)
        {
            this.BindingContext = vm;
            InitializeComponent();
            RegisterRoutes();
        }
        private void RegisterRoutes()
        {
            Routing.RegisterRoute("Approve", typeof(ApprovePuzzlesPage));
            Routing.RegisterRoute("Create", typeof(CreateLevelsPage));
            Routing.RegisterRoute("Select", typeof(LevelSelectPage));
            Routing.RegisterRoute("Profile", typeof(ProfilePage));
            Routing.RegisterRoute("Logout", typeof(LogoutPage));
            Routing.RegisterRoute("Game", typeof(GamePage));
            InitializeComponent();
        }
    }
}
