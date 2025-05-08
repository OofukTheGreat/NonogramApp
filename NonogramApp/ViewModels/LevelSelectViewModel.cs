using Microsoft.Extensions.DependencyInjection;
using NonogramApp.Models;
using NonogramApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NonogramApp.Services;
using System.Threading;

namespace NonogramApp.ViewModels
{
    public class LevelSelectViewModel : ViewModelBase
    {
        private IServiceProvider serviceProvider;
        private NonogramService service;
        public LevelSelectViewModel(IServiceProvider serviceProvider, NonogramService service)
        {
            this.serviceProvider = serviceProvider;
            this.service = service;
            GoToGameCommand = new Command(GoToGame);
            SetSizeFilters();
            SetLevels();           
        }
        public async Task SetLevels()
        {
            List<LevelDTO> templevels = await service.GetApprovedLevels();
            Levels = new ObservableCollection<LevelDTO>();
            foreach (LevelDTO l in templevels)
            {
                Levels.Add(l);
            }
            SelectedSize = "5x5";
        }
        private ObservableCollection<LevelDTO> levels;
        public ObservableCollection<LevelDTO> Levels
        {
            get => levels;
            set
            {
                levels = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<LevelDTO> filterdLevels;
        public ObservableCollection<LevelDTO> FilterdLevels
        {
            get => filterdLevels;
            set
            {
                filterdLevels = value;
                OnPropertyChanged();
            }
        }
        private string selectedSize;
        public string SelectedSize
        {
            get
            {
                return selectedSize;
            }
            set
            {
                selectedSize = value;
                OnPropertyChanged();
                Filterlevels();
            }
        }
        private ObservableCollection<string> sizeFilters;
        public ObservableCollection<string> SizeFilters
        {
            get => sizeFilters;
            set
            {
                sizeFilters = value;
                OnPropertyChanged();
            }
        }
        public void SetSizeFilters()
        {
            SizeFilters = new ObservableCollection<string>()
            {
                "5x5",
                "10x10",
                "15x15",
                "20x20",
                "25x25"
            };

        }
        public void Filterlevels()
        {
            FilterdLevels = new ObservableCollection<LevelDTO>();
            foreach (LevelDTO l in levels)
            {
                if (int.Parse((selectedSize.Substring(0, ((selectedSize.Length - 1) / 2)))) == l.Size)
                    FilterdLevels.Add(l);
            }
        }
        public ICommand GoToGameCommand { get; }
        public void GoToGame()
        {
            InServerCall = true;
            ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<GamePage>());
            InServerCall = false;
        }

        #region Single Selection
        private LevelDTO selectedLevel;
        public LevelDTO SelectedLevel
        {
            get => selectedLevel;
            set
            {
                selectedLevel = value;
                OnPropertyChanged();
                if (selectedLevel != null)
                    OnSingleSelectLevel();
            }
        }


        async void OnSingleSelectLevel()
        {
            var navParam = new Dictionary<string, object>()
                {
                    { "Level",SelectedLevel }
                };
            InServerCall = true;
            await Shell.Current.GoToAsync("Game", navParam);
            InServerCall = false;
            SelectedLevel = null;
        }


        #endregion
    }
}
