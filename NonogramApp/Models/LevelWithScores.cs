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
        public string? HighScore { get; set; }
        public string? CurrentScore { get; set; }
        public RowDefinitionCollection Rows { get; set; }
        public ColumnDefinitionCollection Columns { get; set; }
        public ObservableCollection<Tile> Tiles { get; set; }
        public LevelWithScores(LevelDTO level, string highScore, string currentScore)
        {
            Level = level;
            if (highScore != null) HighScore = highScore;
            else HighScore = "-";
            if (currentScore != null) CurrentScore = currentScore;
            else CurrentScore = "-";
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
