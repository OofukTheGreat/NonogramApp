using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramApp.Models
{
    public class LeaderboardScore
    {
        ScoreDTO Score { get; set; }
        int Index { get; set; }
        string TextColor { get; set; }
        public LeaderboardScore(ScoreDTO score, int index)
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
                    TextColor = "Bronze";
                    break;
                default:
                    TextColor = "Black";
                    break;
            }
        }
    }
}
