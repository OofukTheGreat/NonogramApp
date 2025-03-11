using Microsoft.Extensions.DependencyInjection;
using NonogramApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NonogramApp.ViewModels
{
    public class LevelSelectViewModel : ViewModelBase
    {
        private IServiceProvider serviceProvider;
        public LevelSelectViewModel(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            GoToGameCommand = new Command(GoToGame);
        }
        public ICommand GoToGameCommand { get; }
        public void GoToGame()
        {
            ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<GamePage>());
        }
    }
}
