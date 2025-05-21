using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NonogramApp.Models;
using NonogramApp.Views;
using NonogramApp.Services;
using System.Threading.Tasks;

namespace NonogramApp.ViewModels
{
    public class AppShellViewModel:ViewModelBase
    {
        public AppShellViewModel()
        {
            if (((App)Application.Current).LoggedInUser.IsAdmin) IsAdmin = true;
        }
        private bool isAdmin;
        public bool IsAdmin
        {
            get
            {
                return isAdmin;
            }
            set
            {
                isAdmin = value;
                OnPropertyChanged();
            }
        }
    }
}
