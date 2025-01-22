using NonogramApp.Models;
using NonogramApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace NonogramApp.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private NonogramService service;
        private IServiceProvider serviceProvider;
        public GameViewModel(NonogramService service, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.service = service;
            Level = new LevelDTO(1, "placeholder", "placeholder", 15, 1,1);
        }
        private int boardSize;
        public int BoardSize
        {
            get
            {
                return boardSize;
            }
            set
            {
                boardSize = value;
                OnPropertyChanged(nameof(BoardSize));
            }
        }
        private ObservableCollection<Tile> tiles;
        public ObservableCollection<Tile> Tiles
        {
            get
            {
                return tiles;
            }
            set
            {
                tiles = value;
                OnPropertyChanged(nameof(Tiles));
            }
        }
        private int selectedRow;
        public int SelectedRow
        {
            get
            {
                return selectedRow;
            }
            set
            {
                selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }
        private int selectedColumn;
        public int SelectedColumn
        {
            get
            {
                return selectedColumn;
            }
            set
            {
                selectedColumn = value;
                OnPropertyChanged(nameof(SelectedColumn));
            }
        }
        private LevelDTO level;
        public LevelDTO Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
                OnPropertyChanged(nameof(Level));
                ExpandGrid(Level.DifficultyId);
                BuildBoard(Level.DifficultyId);
            }
        }
        private RowDefinitionCollection rows;
        public RowDefinitionCollection Rows
        {
            get
            {
                return rows;
            }
            set
            {
                rows = value;
                OnPropertyChanged(nameof(Rows));
            }
        }
        private ColumnDefinitionCollection columns;
        public ColumnDefinitionCollection Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
                OnPropertyChanged(nameof(Columns));
            }
        }
        public void ExpandGrid(int size)
        {
            Rows = new();
            Columns = new();
            Rows.Add(new RowDefinition(new GridLength(6, GridUnitType.Star)));
            Columns.Add(new ColumnDefinition(new GridLength(6, GridUnitType.Star)));
            for (int i = 0; i < size; i++)
            {
                Rows.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
                Columns.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            }
        }
        public void BuildBoard(int size)
        {

        }
    }

}
