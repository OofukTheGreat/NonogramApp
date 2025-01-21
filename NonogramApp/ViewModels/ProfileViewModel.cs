using NonogramApp.Models;
using NonogramApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramApp.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private NonogramService service;
        private readonly IServiceProvider serviceProvider;
        public ProfileViewModel(NonogramService service, IServiceProvider serviceProvider)
        {
            this.service = service;
            this.serviceProvider = serviceProvider;
            LoggedUser = ((App)Application.Current).LoggedInUser;
        }
        private PlayerDTO loggedUser;
        public PlayerDTO LoggedUser
        {
            get
            {
                return loggedUser;
            }
            set
            {
                loggedUser = value;
                OnPropertyChanged(nameof(LoggedUser));
            }
        }
    }
}
