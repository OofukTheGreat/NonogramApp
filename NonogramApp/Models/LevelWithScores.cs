using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramApp.Models
{
    public class LevelWithScores
    {
        public LevelDTO Level { get; set; }
        public ScoreDTO? HighScore { get; set; }
        public bool HasHighScore { get; set; }
        public bool NoHighScore { get; set; }
        public ScoreDTO? CurrentScore { get; set; }
        public bool HasCurrentScore { get; set; }
        public bool NoCurrentScore { get; set; }
        public RowDefinitionCollection Rows { get; set; }
        public ColumnDefinitionCollection Columns { get; set; }
        public ObservableCollection<Tile> Tiles { get; set; }
        public LevelWithScores(LevelDTO level, ScoreDTO highScore, ScoreDTO currentScore)
        {
            Level = level;
            HighScore = highScore;
            if (highScore != null)
            {
                HasHighScore = true;
                NoHighScore = false;
            }
            else
            {
                HasHighScore = false;
                NoHighScore = true;
            }
            CurrentScore = currentScore;
            if (currentScore != null)
            {
                HasCurrentScore = true;
                NoCurrentScore = false;
            }
            else
            {
                HasCurrentScore = false;
                NoCurrentScore = true;
            }
            Rows = new();
            Columns = new();
            for (int i = 0; i < level.Size; i++)
            {
                Rows.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
                Columns.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            }
            Game game = new Game(Level);
            Tiles = new ObservableCollection<Tile>(game.GetBoardAsList(Level.Size));
        }
    }
}
