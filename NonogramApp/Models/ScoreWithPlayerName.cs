using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramApp.Models
{
    public class ScoreWithPlayerName
    {
        public ScoreDTO Score { get; set; }
        public string Name { get; set; }
        public ScoreWithPlayerName(ScoreDTO s, string name)
        {
            this.Score = s;
            this.Name = name;
        }
    }
}
