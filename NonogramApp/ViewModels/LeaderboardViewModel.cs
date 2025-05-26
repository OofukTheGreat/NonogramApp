using NonogramApp.Models;
using NonogramApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using System.Windows.Input;
using NonogramApp.Views;
using System.Diagnostics;


namespace NonogramApp.ViewModels
{
    public partial class GameViewModel:ViewModelBase
    {
        private async void InitData()
        {
            await GetPlayers();
            await GetScoresByList();
        }
        private async Task GetPlayers()
        {
            List<PlayerDTO> p = await service.GetPlayers();
            players = new(p);
        }
        private ScoreWithPlayerData ScoresMin(List<ScoreWithPlayerData> scores)
        {
            int min = int.MaxValue;
            foreach (ScoreWithPlayerData s in scores)
            {
                if (s.Score.Time < min) min = s.Score.Time;
            }
            return scores.Where(s => s.Score.Time == min).FirstOrDefault();
        }
        private async Task GetScoresByList()
        {
            Scores.Clear();
            List<ScoreDTO> _scores = await service.GetScoresByList(Level.LevelId);
            foreach (ScoreDTO s in _scores)
            {
                s.Seconds = s.Time % 60;
                s.Minutes = s.Time / 60 % 60;
                s.Hours = s.Time / 3600;
            }
            foreach (ScoreDTO s in _scores)
            {
                string name = players.Where(x => x.Id == s.PlayerId).FirstOrDefault().DisplayName;
                string url = players.Where(x => x.Id == s.PlayerId).FirstOrDefault().FullUrl;
                Scores.Add(new ScoreWithPlayerData(s, name, url));
            }
            Scores = new(Scores.OrderBy(s => s.Score.Time).ToList());
            VisScores = new ObservableCollection<LeaderboardScore>();
            try
            { 
                foreach (ScoreWithPlayerData s in Scores)
                {
                    int index = Scores.IndexOf(s);
                    VisScores.Add(new LeaderboardScore(s, index+1));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error while building VisScores: " + ex.Message);
            }
            VisScores = new(VisScores.OrderBy(s => s.Score.Score.Time).ToList());
            //foreach (ScoreWithPlayerData s in scores)
            //{
            //    Scores.Add(s);
            //}
            //scores.Clear();
            //scores = Scores.ToList();
        }
        private List<PlayerDTO> players;
        //private List<ScoreWithPlayerData> scores;
        private List<ScoreWithPlayerData> scores;
        public List<ScoreWithPlayerData> Scores
        {
            get => scores;
            set
            {
                scores = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<LeaderboardScore> visScores;
        public ObservableCollection<LeaderboardScore> VisScores
        {
            get => visScores;
            set
            {
                visScores = value;
                OnPropertyChanged();
            }
        }
        public ICommand ClosePopupCommand { get; set; }
        public void ClosePopup()
        {
            ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<LevelSelectPage>());
        }
    }
}
