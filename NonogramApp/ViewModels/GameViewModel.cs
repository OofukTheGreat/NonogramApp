using NonogramApp.Models;
using NonogramApp.Services;
using NonogramApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;

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
            Level = new LevelDTO(1, "Heart", "11111.05.05.131.212.", 5, 1, 1);
            UpCommand = new Command(Up);
            DownCommand = new Command(Down);
            LeftCommand = new Command(Left);
            RightCommand = new Command(Right);
            ColorCommand = new Command(ColorTile);
            SelectedX = 1;
            SelectedY = 1;
            CreateGame();
            time = 0;
            Timer();
        }
        public ICommand ColorCommand { get; set; }
        public ICommand UpCommand {get; set;}
        public ICommand DownCommand {get; set;}
        public ICommand LeftCommand {get; set;}
        public ICommand RightCommand { get; set;}
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
        private void Up()
        {
            int temp = SelectedY;
            SelectedY -= 1;
            if (SelectedY < 1) SelectedY = Level.DifficultyId;
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#FF0000");
            Tiles.Where(T => T.X == SelectedX && T.Y == temp).FirstOrDefault().BorderColor = Color.FromArgb("#000000");
        }
        private void Down()
        {
            int temp = SelectedY;
            SelectedY += 1;
            if (SelectedY > Level.DifficultyId) SelectedY = 1;
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#FF0000");
            Tiles.Where(T => T.X == SelectedX && T.Y == temp).FirstOrDefault().BorderColor = Color.FromArgb("#000000");
        }
        private void Left()
        {
            int temp = SelectedX;
            SelectedX -= 1;
            if (SelectedX < 1) SelectedX = Level.DifficultyId;
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#FF0000");
            Tiles.Where(T => T.X == temp && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#000000");
        }
        private void Right()
        {
            int temp = SelectedX;
            SelectedX += 1;
            if (SelectedX > Level.DifficultyId) SelectedX = 1;
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#FF0000");
            Tiles.Where(T => T.X == temp && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#000000");
        }
        private void ColorTile()
        {
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().FlipColor();
            bool hasWon = true;
            foreach (Tile T in Tiles)
            {
                if (T.CurrentColor != T.TrueColor) hasWon = false;
            }
            if (hasWon==true ) ((App)Application.Current).MainPage = new NavigationPage(serviceProvider.GetService<WelcomePage>());
        }
    }

}
