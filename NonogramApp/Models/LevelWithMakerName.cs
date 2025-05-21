using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramApp.Models
{
    public class LevelWithMakerName
    {
        public LevelDTO Level { get; set; }
        public PlayerDTO Maker { get; set; }
        public RowDefinitionCollection Rows { get; set; }
        public ColumnDefinitionCollection Columns { get; set; }
        public ObservableCollection<Tile> Tiles { get; set; }
        public LevelWithMakerName(LevelDTO level, PlayerDTO maker)
        {
            Level = level;
            Maker = maker;
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
