using NonogramApp.Models;
using NonogramApp.Services;
using NonogramApp.Views;

namespace NonogramApp
{
    public partial class App : Application
    {
        //Application level variables
        public PlayerDTO? LoggedInUser { get; set; }
        private NonogramService service;
        public App(IServiceProvider serviceProvider, NonogramService service)
        {   
            this.service = service;
            InitializeComponent();
            LoggedInUser = null;
            //Start with the Login View
            MainPage = new NavigationPage(serviceProvider.GetService<WelcomePage>());
        }
    }
}
