using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramApp.Models
{
    public class LeaderboardScore
    {
        public ScoreWithPlayerData Score { get; set; }
        public int Index { get; set; }
        public string TextColor { get; set; }
        public LeaderboardScore(ScoreWithPlayerData score, int index)
        {
            Score = score;
            Index = index;
            switch (index)
            {
                case 1:
                    TextColor = "Gold";
                    break;
                case 2:
                    TextColor = "Silver";
                    break;
                case 3:
                    TextColor = "Brown";
                    break;
                default:
                    TextColor = "White";
                    break;
            }
        }
    }
}
