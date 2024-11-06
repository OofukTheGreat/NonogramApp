using NonogramApp.Views;

namespace NonogramApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute("Login", typeof(LoginPage));
            Routing.RegisterRoute("Signup", typeof(SignupPage));
            InitializeComponent();
        }
    }
}
