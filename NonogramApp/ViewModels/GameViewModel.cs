﻿using NonogramApp.Models;
using NonogramApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Timers;

namespace NonogramApp.ViewModels
{
    //[QueryProperty(nameof(Level), "Level")]
    public class GameViewModel : ViewModelBase
    {
        private NonogramService service;
        private IServiceProvider serviceProvider;
        public GameViewModel(NonogramService service, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.service = service;
            Level = new LevelDTO(1, "Heart", "11111.05.05.131.212.", 15, 1,1);
            CreateGame();
            time = 0;
            Timer();
        }
        private double time;
        public double Time
        {
            get => Math.Round(time,3); // second number is how many decimels it rounds to
            set
            {
                time = value;
                OnPropertyChanged();
            }
        }
        private Game game;
        public Game Game
        {
            get
            {
                return game;
            }
            set
            {
                game = value;
                OnPropertyChanged(nameof(Game));
            }
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
        public async void ExpandGrid(int size)
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
        private async void CreateGame()
        {
            Game = new Game(Level);
            TileArrayToList(Level.DifficultyId);
        }
        private async Task TileArrayToList(int size)
        {
            Tiles = new ObservableCollection<Tile>(Game.GetBoardAsList(size));
        }

        private async void Timer()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(UpdateTime);
            aTimer.Interval = 10; //how often it triggers (in milliseconds)
            aTimer.Enabled = true;
        }

        private void UpdateTime(object source, ElapsedEventArgs e)
        {
            this.time += 0.01; //how much gets added every trigger (in seconds)
            OnPropertyChanged("Time");
        }
    }

}
