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

namespace NonogramApp.ViewModels
{
    public class LogoutPageViewModel:ViewModelBase
    {
        private NonogramService service;
        private IServiceProvider serviceProvider;
        private async void OnLogin()
        {
            ((App)Application.Current).LoggedInUser = null;
            //Navigate to the welcome page
            ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<WelcomePage>());
        }
    }
}
