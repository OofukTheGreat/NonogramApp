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
            SetScores();
        }
        public async void SetLevels()
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
        private List<ScoreDTO> scores;
        public List<ScoreDTO> Scores
        {
            get
            {
                return scores;
            }
            set
            {
                scores = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<LevelDTO> filteredLevels;
        public ObservableCollection<LevelDTO> FilteredLevels
        {
            get => filteredLevels;
            set
            {
                filteredLevels = value;
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
        private bool filterUnplayed;
        public bool FilterUnplayed
        {
            get
            {
                return filterUnplayed;
            }
            set
            {
                filterUnplayed = value;
                OnPropertyChanged();
                Filterlevels();
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
        public async void SetScores()
        {
            List<ScoreDTO> tempscores = await service.GetScoresByPlayer(((App)Application.Current).LoggedInUser.Id);
            Scores = new();
            foreach (ScoreDTO s in tempscores)
            {
                Scores.Add(s);
            }
        }
        public void Filterlevels()
        {
            FilteredLevels = new ObservableCollection<LevelDTO>();
            if (!FilterUnplayed)
            {
                foreach (LevelDTO l in levels)
                {
                    if (int.Parse((selectedSize.Substring(0, ((selectedSize.Length - 1) / 2)))) == l.Size)
                        FilteredLevels.Add(l);
                }
            }
            else
            {
                foreach (LevelDTO l in levels)
                {
                    if (int.Parse((selectedSize.Substring(0, ((selectedSize.Length - 1) / 2)))) == l.Size && !Scores.Any(s=>s.LevelId==l.LevelId))
                        FilteredLevels.Add(l);
                }
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
