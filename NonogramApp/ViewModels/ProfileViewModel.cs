using CommunityToolkit.Maui.Core.Extensions;
using NonogramApp.Models;
using NonogramApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            VisLevels = new ObservableCollection<LevelWithScores>();
            LoggedUser = ((App)Application.Current).LoggedInUser;
            InitData();
        }
        public async void InitData()
        {
            await SetLevels();
            await SetScores();
            await SetLevelsWithScores();
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
        }
        public async Task SetScores()
        {
            List<ScoreDTO> tempscores = await service.GetPlayerWinningScores(((App)Application.Current).LoggedInUser.Id);
            Scores = new();
            if (tempscores != null)
            {
                foreach (ScoreDTO s in tempscores)
                {
                    Scores.Add(s);
                }
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
    }
}
