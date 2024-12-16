using NonogramApp.Services;
using NonogramApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NonogramApp.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {
        private NonogramService service;
        private IServiceProvider serviceProvider;
        public WelcomeViewModel(NonogramService service, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.service = service;
            LoginCommand = new Command(OnLogin);
            SignupCommand = new Command(OnSignup);
        }
        public ICommand LoginCommand { get; set; }
        public ICommand SignupCommand { get; set; }
        private void OnSignup()
        {
            ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<SignupPage>());
        }
        private void OnLogin()
        {
            ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<LoginPage>());
        }
    }
}
