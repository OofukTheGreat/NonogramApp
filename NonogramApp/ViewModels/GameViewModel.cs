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
            Level = new LevelDTO(1, "Cherry", "64.5221.41122.32212.21412.13231.012322.011413.0415.12331.", 10, 1, 1);
            UpCommand = new Command(Up);
            DownCommand = new Command(Down);
            LeftCommand = new Command(Left);
            RightCommand = new Command(Right);
            ColorCommand = new Command(ColorTile);
            MarkCommand = new Command(MarkTile);
            SizePlusOne = Level.DifficultyId + 1;
            SelectedX = 0;
            SelectedY = 0;
            CreateGame();
            time = 0;
            Timer();
        }
        public ICommand ColorCommand { get; set; }
        public ICommand MarkCommand { get; set; }
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
        private int sizePlusOne;
        public int SizePlusOne
        {
            get
            {
                return sizePlusOne;
            }
            set
            {
                sizePlusOne = value;
                OnPropertyChanged(nameof(SizePlusOne));
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
        private ObservableCollection<Hint> labels;
        public ObservableCollection<Hint> Labels
        {
            get
            {
                return labels;
            }
            set
            {
                labels = value;
                OnPropertyChanged(nameof(Labels));
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
        private RowDefinitionCollection exRows;
        public RowDefinitionCollection ExRows
        {
            get
            {
                return exRows;
            }
            set
            {
                exRows = value;
                OnPropertyChanged(nameof(ExRows));
            }
        }
        private ColumnDefinitionCollection exColumns;
        public ColumnDefinitionCollection ExColumns
        {
            get
            {
                return exColumns;
            }
            set
            {
                exColumns = value;
                OnPropertyChanged(nameof(ExColumns));
            }
        }
        public async void ExpandGrid(int size)
        {
            Rows = new();
            ExRows = new();
            Columns = new();
            ExColumns = new();
            ExRows.Add(new RowDefinition(new GridLength(50, GridUnitType.Absolute)));
            ExColumns.Add(new ColumnDefinition(new GridLength(50, GridUnitType.Absolute)));
            for (int i = 0; i < size; i++)
            {
                Rows.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
                ExRows.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
                Columns.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
                ExColumns.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            }
        }
        private async void CreateGame()
        {
            Game = new Game(Level);
            TileArrayToList(Level.DifficultyId);
            LabelsToList(Level.DifficultyId);
        }
        private async Task TileArrayToList(int size)
        {
            Tiles = new ObservableCollection<Tile>(Game.GetBoardAsList(size));
        }
        private async Task LabelsToList(int size)
        {
            Labels = new ObservableCollection<Hint>(Game.GetLayoutList(size));
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
            if (SelectedY < 0) SelectedY = Level.DifficultyId-1;
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#FF0000");
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderWidth = 4;
            Tiles.Where(T => T.X == SelectedX && T.Y == temp).FirstOrDefault().BorderColor = Color.FromArgb("#000000");
            Tiles.Where(T => T.X == SelectedX && T.Y == temp).FirstOrDefault().BorderWidth = 1;
        }
        private void Down()
        {
            int temp = SelectedY;
            SelectedY += 1;
            if (SelectedY > Level.DifficultyId-1) SelectedY = 0;
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#FF0000");
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderWidth = 4;
            Tiles.Where(T => T.X == SelectedX && T.Y == temp).FirstOrDefault().BorderColor = Color.FromArgb("#000000");
            Tiles.Where(T => T.X == SelectedX && T.Y == temp).FirstOrDefault().BorderWidth = 1;
        }
        private void Left()
        {
            int temp = SelectedX;
            SelectedX -= 1;
            if (SelectedX < 0) SelectedX = Level.DifficultyId-1;
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#FF0000");
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderWidth = 4;
            Tiles.Where(T => T.X == temp && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#000000");
            Tiles.Where(T => T.X == temp && T.Y == SelectedY).FirstOrDefault().BorderWidth = 1;
        }
        private void Right()
        {
            int temp = SelectedX;
            SelectedX += 1;
            if (SelectedX > Level.DifficultyId-1) SelectedX = 0;
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#FF0000");
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().BorderWidth = 4;
            Tiles.Where(T => T.X == temp && T.Y == SelectedY).FirstOrDefault().BorderColor = Color.FromArgb("#000000");
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
            if (hasWon==true ) ((App)Application.Current).MainPage = new NavigationPage(serviceProvider.GetService<WelcomePage>());
        }
        private void MarkTile()
        {
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().Mark();
        }
    }

}
