using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramApp.Models
{
    public class ScoreWithPlayerData
    {
        public ScoreDTO Score { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public ScoreWithPlayerData(ScoreDTO s, string name, string url)
        {
            this.Score = s;
            this.Name = name;
            this.URL = url;
        }
    }
}
