using NonogramApp.Models;
using NonogramApp.Services;
using NonogramApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NonogramApp.ViewModels
{
    public class CreateLevelsViewModel : ViewModelBase
    {
        private NonogramService service;
        private IServiceProvider serviceProvider;
        public CreateLevelsViewModel(NonogramService service, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.service = service;
            UpCommand = new Command(Up);
            DownCommand = new Command(Down);
            LeftCommand = new Command(Left);
            RightCommand = new Command(Right);
            ExitCommand = new Command(OnExit);
            SaveCommand = new Command(OnExit);
            ColorCommand = new Command(ColorTile);
            BoardSize = 5;
            SelectedX = 0;
            SelectedY = 0;
            CreateGame();
        }
        #region (Instance)Variables
        public ICommand ColorCommand { get; set; }
        public ICommand UpCommand { get; set; }
        public ICommand DownCommand { get; set; }
        public ICommand LeftCommand { get; set; }
        public ICommand RightCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand SaveCommand { get; set; }
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
        private int selectedX;
        public int SelectedX
        {
            get
            {
                return selectedX;
            }
            set
            {
                selectedX = value;
                OnPropertyChanged(nameof(SelectedX));
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
                ExpandGrid(BoardSize);
                OnPropertyChanged(nameof(BoardSize));
            }
        }
        private int selectedY;
        public int SelectedY
        {
            get
            {
                return selectedY;
            }
            set
            {
                selectedY = value;
                OnPropertyChanged(nameof(SelectedY));
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
        #endregion
        #region GameCreation
        public async void ExpandGrid(int size)
        {
            Rows = new();
            Columns = new();
            for (int i = 0; i < size; i++)
            {
                Rows.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
                Columns.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            }
        }
        private async void CreateGame()
        {
            Game = new Game(BoardSize);
            TileArrayToList(BoardSize);
        }
        private async Task TileArrayToList(int size)
        {
            Tiles = new ObservableCollection<Tile>(Game.GetBoardAsList(size));
            int i = 0;
        }
        #endregion
        #region GameOperation
        private void Up()
        {
            int temp = SelectedY;
            SelectedY -= 1;
            if (SelectedY < 0) SelectedY = BoardSize - 1;
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#FF0000");
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderWidth = 4;
            Tiles.Where(T => T.X == SelectedX && T.Y == temp).FirstOrDefault().BorderColor = Color.FromArgb("#808080");
            Tiles.Where(T => T.X == SelectedX && T.Y == temp).FirstOrDefault().BorderWidth = 1;
        }
        private void Down()
        {
            int temp = SelectedY;
            SelectedY += 1;
            if (SelectedY > BoardSize - 1) SelectedY = 0;
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#FF0000");
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderWidth = 4;
            Tiles.Where(T => T.X == SelectedX && T.Y == temp).FirstOrDefault().BorderColor = Color.FromArgb("#808080");
            Tiles.Where(T => T.X == SelectedX && T.Y == temp).FirstOrDefault().BorderWidth = 1;
        }
        private void Left()
        {
            int temp = SelectedX;
            SelectedX -= 1;
            if (SelectedX < 0) SelectedX = BoardSize - 1;
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#FF0000");
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderWidth = 4;
            Tiles.Where(T => T.X == temp && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#808080");
            Tiles.Where(T => T.X == temp && T.Y == SelectedY).FirstOrDefault().BorderWidth = 1;
        }
        private void Right()
        {
            int temp = SelectedX;
            SelectedX += 1;
            if (SelectedX > BoardSize - 1) SelectedX = 0;
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#FF0000");
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderWidth = 4;
            Tiles.Where(T => T.X == temp && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#808080");
            Tiles.Where(T => T.X == temp && T.Y == SelectedY).FirstOrDefault().BorderWidth = 1;
        }
        private void ColorTile()
        {
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().Blacken();
            bool hasWon = true;
            foreach (Tile T in Tiles)
            {
                if (T.CurrentColor != T.TrueColor) hasWon = false;
            }
            if (hasWon) GameWon();
        }
        #endregion
        #region PostGame
        private async void GameWon()
        {
            bool f = await SaveProgress(true);
        }
        private async void OnExit()
        {
            bool f = await SaveProgress(false);
            // Navigate to the Register View page
            ((App)Application.Current).MainPage.Navigation.PopAsync();
        }
        private async Task<bool> SaveProgress(bool haswon)
        {
            string layout = this.service.TileArrayToLayout(this.service.TileListToArray(BoardSize, Tiles));
            return true;
        }
        #endregion
    }
}
