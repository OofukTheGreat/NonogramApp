using NonogramApp.Models;
using NonogramApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit;
using System.Collections.ObjectModel;


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
            scores.Clear();
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
                scores.Add(new ScoreWithPlayerData(s, name, url));
            }
            List<ScoreWithPlayerData> scores_ = new List<ScoreWithPlayerData>();
            foreach (ScoreWithPlayerData s in scores)
            {
                Scores.Add(s);
            }
            Scores = new(Scores.OrderBy(s => s.Score.Time).ToList());
            scores = Scores.ToList();

        }
        private List<PlayerDTO> players;
        private List<ScoreWithPlayerData> scores;
        public ObservableCollection<ScoreWithPlayerData> Scores { get; set; }
    }
}
