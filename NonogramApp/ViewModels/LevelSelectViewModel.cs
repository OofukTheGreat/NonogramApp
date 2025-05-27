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
using CommunityToolkit.Maui.Core.Extensions;

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
            VisLevels = new ObservableCollection<LevelWithScores>();
            FilteredLevels = new ObservableCollection<LevelWithScores>();
            SetSizeFilters();
            InitData();
        }
        public ICommand GoToGameCommand { get; }
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
        private ObservableCollection<LevelWithScores> visLevels;
        public ObservableCollection<LevelWithScores> VisLevels
        {
            get => visLevels;
            set
            {
                visLevels = value;
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
        private ObservableCollection<LevelWithScores> filteredLevels;
        public ObservableCollection<LevelWithScores> FilteredLevels
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
                FilterLevels();
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
                FilterLevels();
            }
        }
        public async void InitData()
        {
            await SetLevels();
            await SetScores();
            await SetLevelsWithScores();
            FilterLevels();
        }
        public async Task SetLevels()
        {
            List<LevelDTO> templevels = await service.GetApprovedLevels();
            Levels = new ObservableCollection<LevelDTO>();
            foreach (LevelDTO l in templevels)
            {
                Levels.Add(l);
            }
            ObservableCollection<LevelDTO> orderlvels = Levels.OrderBy(l => l.Size).ToObservableCollection();
            Levels = orderlvels.ToObservableCollection();
            SelectedSize = "Any";
        }
        public void SetSizeFilters()
        {
            SizeFilters = new ObservableCollection<string>()
            {
                "Any",
                "5x5",
                "10x10",
                "15x15",
                "20x20",
                "25x25"
            };
        }
        public async Task SetScores()
        {
            List<ScoreDTO> tempscores = await service.GetScoresByPlayer(((App)Application.Current).LoggedInUser.Id);
            Scores = new();
            foreach (ScoreDTO s in tempscores)
            {
                Scores.Add(s);
            }
        }
        public async Task SetLevelsWithScores()
        {
            VisLevels = new();
            foreach (LevelDTO l in Levels)
            {
                VisLevels.Add(new LevelWithScores(l, Scores.FirstOrDefault(s => s.LevelId == l.LevelId && s.HasWon == true), Scores.FirstOrDefault(s => s.LevelId == l.LevelId && s.HasWon == false)));
            }
        }
        public void FilterLevels()
        {
            FilteredLevels = new ObservableCollection<LevelWithScores>();
            if (!FilterUnplayed)
            {
                if (SelectedSize == "Any")
                {
                    foreach (LevelWithScores l in VisLevels)
                    {
                        FilteredLevels.Add(l);
                    }
                }
                else
                {
                    foreach (LevelWithScores l in VisLevels)
                    {
                        if (int.Parse((SelectedSize.Substring(0, ((SelectedSize.Length - 1) / 2)))) == l.Level.Size)
                            FilteredLevels.Add(l);
                    }
                }
            }
            else
            {
                if (SelectedSize == "Any")
                {
                    foreach (LevelWithScores l in VisLevels)
                    {
                        if (!Scores.Any(s => s.LevelId == l.Level.LevelId && s.HasWon == true))
                            FilteredLevels.Add(l);
                    }
                }
                else
                {
                    foreach (LevelWithScores l in VisLevels)
                    {
                        if (int.Parse((SelectedSize.Substring(0, ((selectedSize.Length - 1) / 2)))) == l.Level.Size && !Scores.Any(s => s.LevelId == l.Level.LevelId && s.HasWon == true))
                            FilteredLevels.Add(l);
                    }
                }
            }
            FilteredLevels.OrderBy(l => l.Level.Size);
        }
        public void GoToGame()
        {
            InServerCall = true;
            ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<GamePage>());
            InServerCall = false;
        }
        #region Single Selection
        private LevelWithScores selectedLevel;
        public LevelWithScores SelectedLevel
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
                    { "Level",SelectedLevel.Level },
                    { "Score",SelectedLevel.CurrentScore }
                };
            InServerCall = true;
            await Shell.Current.GoToAsync("Game", navParam);
            InServerCall = false;
            SelectedLevel = null;
        }


        #endregion
    }
}
