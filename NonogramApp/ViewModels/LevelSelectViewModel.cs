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
        public async void InitData()
        {
            await SetLevels();
            await SetScores();
            await SetLevelsWithScores();
            FilterLevels();
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
                string highscorestring = "";
                string currentscorestring = "";
                string highhours = "";
                string highminutes = "";
                string highseconds = "";
                string currenthours = "";
                string currentminutes = "";
                string currentseconds = "";
                ScoreDTO highscore = Scores.FirstOrDefault(s => s.LevelId == l.LevelId && s.HasWon == true);
                if (highscore != null)
                {
                    if (highscore.Time / 3600 < 10) highhours = "0";
                    else highhours = "";
                    highhours += $"{highscore.Time / 3600}";
                    if ((highscore.Time / 60) % 60 < 10) highminutes = "0";
                    else highminutes = "";
                    highminutes += $"{(highscore.Time / 60) % 60}";
                    if (highscore.Time % 60 < 10) highseconds = "0";
                    else highseconds = "";
                    highseconds += $"{highscore.Time % 60}";
                    highscorestring = $"{highhours}:{highminutes}:{highseconds}";
                }
                else highscorestring = null;
                ScoreDTO currentscore = Scores.FirstOrDefault(s => s.LevelId == l.LevelId && s.HasWon == false);
                if (currentscore != null)
                {
                    if (currentscore.Time / 3600 < 10) currenthours = "0";
                    else currenthours = "";
                    currenthours += $"{currentscore.Time / 3600}";
                    if ((currentscore.Time / 60) % 60 < 10) currentminutes = "0";
                    else currentminutes = "";
                    currentminutes += $"{(currentscore.Time / 60) % 60}";
                    if (currentscore.Time % 60 < 10) currentseconds = "0";
                    else currentseconds = "";
                    currentseconds += $"{currentscore.Time % 60}";
                    currentscorestring = $"{currenthours}:{currentminutes}:{currentseconds}";
                }
                else currentscorestring = null;
                    VisLevels.Add(new LevelWithScores(l, highscorestring, currentscorestring));
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
                        if (!Scores.Any(s => s.LevelId == l.Level.LevelId))
                            FilteredLevels.Add(l);
                    }
                }
                else
                {
                    foreach (LevelWithScores l in VisLevels)
                    {
                        if (int.Parse((SelectedSize.Substring(0, ((selectedSize.Length - 1) / 2)))) == l.Level.Size && !Scores.Any(s => s.LevelId == l.Level.LevelId))
                            FilteredLevels.Add(l);
                    }
                }
            }
            FilteredLevels.OrderBy(l => l.Level.Size);
        }
        public ICommand GoToGameCommand { get; }
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
                    { "Level",SelectedLevel.Level }
                };
            InServerCall = true;
            await Shell.Current.GoToAsync("Game", navParam);
            InServerCall = false;
            SelectedLevel = null;
        }


        #endregion
    }
}
