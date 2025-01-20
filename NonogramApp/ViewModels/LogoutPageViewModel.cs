using Microsoft.Extensions.DependencyInjection;
using NonogramApp.Models;
using NonogramApp.Services;
using NonogramApp.ViewModels;
using NonogramApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NonogramApp.ViewModels
{
    public class LogoutPageViewModel : ViewModelBase
    {
        private NonogramService service;
        private IServiceProvider serviceProvider;
        public LogoutPageViewModel(NonogramService service, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.service = service;
            LogoutCommand = new Command(OnLogout);
        }
        public ICommand LogoutCommand { get; set; }
        private async void OnLogout()
        {
            ((App)Application.Current).LoggedInUser = null;
            //Navigate to the welcome page
            ((App)Application.Current).MainPage = new NavigationPage(serviceProvider.GetService<WelcomePage>());
        }
    }
}
