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
    [QueryProperty(nameof(Level), "Level")]
    //[QueryProperty(nameof(Score), "Score")]
    public partial class GameViewModel : ViewModelBase
    {
        public event Action<List<string>> OpenPopup;
        private NonogramService service;
        private IServiceProvider serviceProvider;
        public GameViewModel(NonogramService service, IServiceProvider serviceProvider) 
        {
            this.serviceProvider = serviceProvider;
            this.service = service;
            scores = new();
            Scores = new();           
            UpCommand = new Command(Up);
            DownCommand = new Command(Down);
            LeftCommand = new Command(Left);
            RightCommand = new Command(Right);
            ExitCommand = new Command(OnExit);
            ColorCommand = new Command(ColorTile);
            MarkCommand = new Command(MarkTile);
            AlterCommand = new Command((Object o) => AlterTile(o));            
            SelectedX = 0;
            SelectedY = 0;
            IsColoring = true;
            ColorBorder = Color.FromArgb("#808080");
            MarkBorder = Color.FromArgb("#808080");
            MoveBorder = Color.FromArgb("#ffbb00");
            MoveBorder2 = "Red";            
        }
        #region (Instance)Variables
        public ICommand AlterCommand { get; set; }
        public ICommand ColorCommand { get; set; }
        public ICommand MarkCommand { get; set; }
        public ICommand UpCommand {get; set;}
        public ICommand DownCommand {get; set;}
        public ICommand LeftCommand {get; set;}
        public ICommand RightCommand { get; set;}
        public ICommand ExitCommand { get; set;}
        private int time;
        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                Hours = Time / 3600;
                Minutes = (Time / 60) % 60;
                Seconds = Time % 60;
                time = value;
                OnPropertyChanged();
            }
        }
        private int seconds;
        public int Seconds
        {
            get
            {
                return seconds;
            }
            set
            {
                seconds = value;

                OnPropertyChanged();
            }
        }
        private int minutes;
        public int Minutes
        {
            get
            {
                return minutes;
            }
            set
            {
                minutes = value;

                OnPropertyChanged();
            }
        }
        private int hours;
        public int Hours
        {
            get
            {
                return hours;
            }
            set
            {
                hours = value;

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
                OnPropertyChanged();
                ExpandGrid(Level.Size);
                SizePlusOne = Level.Size + 1;
                CreateGame();
                Time = 0;
                Timer();
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
        private bool isColoring;
        public bool IsColoring
        {
            get
            {
                return isColoring;
            }
            set
            {
                isColoring = value;
                OnPropertyChanged(nameof(IsColoring));
            }
        }
        private bool isClicking;
        public bool IsClicking
        {
            get
            {
                return isClicking;
            }
            set
            {
                isClicking = value;
                OnPropertyChanged(nameof(IsClicking));
            }
        }
        private Color colorBorder;
        public Color ColorBorder
        {
            get
            {
                return colorBorder;
            }
            set
            {
                colorBorder = value;
                OnPropertyChanged(nameof(ColorBorder));
            }
        }
        private Color markBorder;
        public Color MarkBorder
        {
            get
            {
                return markBorder;
            }
            set
            {
                markBorder = value;
                OnPropertyChanged(nameof(MarkBorder));
            }
        }
        private Color moveBorder;
        public Color MoveBorder
        {
            get
            {
                return moveBorder;
            }
            set
            {
                moveBorder = value;
                OnPropertyChanged(nameof(MoveBorder));
            }
        }
        private string moveBorder2;
        public string MoveBorder2
        {
            get
            {
                return moveBorder2;
            }
            set
            {
                moveBorder2 = value;
                OnPropertyChanged(nameof(MoveBorder2));
            }
        }
        #endregion
        #region GameCreation
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
            TileArrayToList(Level.Size);
            LabelsToList(Level.Size);
            MarkEmptyRowColumn();
        }
        private async Task TileArrayToList(int size)
        {
            Tiles = new ObservableCollection<Tile>(Game.GetBoardAsList(size));
            int i = 0;
        }
        private async Task LabelsToList(int size)
        {
            Labels = new ObservableCollection<Hint>(Game.GetLayoutList(size));
        }

        private System.Timers.Timer timer;
        private async void Timer()
        {

            System.Timers.Timer aTimer = new System.Timers.Timer();
            timer = aTimer;
            aTimer.Elapsed += new ElapsedEventHandler(UpdateTime);
            aTimer.Interval = 1000; //how often it triggers (in milliseconds)
            aTimer.Start();
        }

        private void UpdateTime(object source, ElapsedEventArgs e)
        {
            this.Time += 1; //how much gets added every trigger (in seconds)
            OnPropertyChanged("Time");
            OnPropertyChanged("Hours");
            OnPropertyChanged("Seconds");
            OnPropertyChanged("Minutes");
        }
        private void MarkEmptyRowColumn()
        {
            int j = 0;
            for (int i = 0; i < Level.Size; i++)
            {
                MarkRowColumn(i, j);
                j++;
            }
        }
        #endregion
        #region GameOperation
        private void Up()
        {
            if (!IsClicking)
            {
                int temp = SelectedY;
                SelectedY -= 1;
                if (SelectedY < 0) SelectedY = Level.Size - 1;
                Game.SelectTile(SelectedX, temp, SelectedX, SelectedY);
            }
            else
            {
                IsClicking = false;
                ColorBorder = Color.FromArgb("#808080");
                MarkBorder = Color.FromArgb("#808080");
                MoveBorder = Color.FromArgb("#ffbb00");
            }
        }
        private void Down()
        {
            if (!IsClicking)
            {
                int temp = SelectedY;
                SelectedY += 1;
                if (SelectedY > Level.Size - 1) SelectedY = 0;
                Game.SelectTile(SelectedX, temp, SelectedX, SelectedY);
            }
            else
            {
                IsClicking = false;
                ColorBorder = Color.FromArgb("#808080");
                MarkBorder = Color.FromArgb("#808080");
                MoveBorder = Color.FromArgb("#ffbb00");
            }
        }
        private void Left()
        {
            if (!IsClicking)
            {
                int temp = SelectedX;
                SelectedX -= 1;
                if (SelectedX < 0) SelectedX = Level.Size - 1;
                Game.SelectTile(temp, SelectedY, SelectedX, SelectedY);
            }
            else
            {
                IsClicking = false;
                ColorBorder = Color.FromArgb("#808080");
                MarkBorder = Color.FromArgb("#808080");
                MoveBorder = Color.FromArgb("#ffbb00");
            }
        }
        private void Right()
        {
            if (!IsClicking)
            {
                int temp = SelectedX;
                SelectedX += 1;
                if (SelectedX > Level.Size - 1) SelectedX = 0;
                Game.SelectTile(temp, SelectedY, SelectedX, SelectedY);
            }
            else
            {
                IsClicking = false;
                ColorBorder = Color.FromArgb("#808080");
                MarkBorder = Color.FromArgb("#808080");
                MoveBorder = Color.FromArgb("#ffbb00");
            }
        }
        private void ColorTile()
        {
            if (!IsClicking)
            {
                Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().Blacken();
                bool hasWon = true;
                foreach (Tile T in Tiles)
                {
                    if (T.CurrentColor != T.TrueColor) hasWon = false;
                }
                MarkRowColumn(SelectedX, SelectedY);
                if (hasWon)
                {
                    timer.Stop();
                    GameWon();
                }
            }
            else
            {
                IsColoring = true;
                ColorBorder = Color.FromArgb("#ffbb00");
                MarkBorder = Color.FromArgb("#808080");
            }
        }
        private void MarkTile()
        {
            if (!IsClicking)
            {
                Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().Mark();
            }
            else
            {
                IsColoring = false;
                MarkBorder = Color.FromArgb("#ffbb00");
                ColorBorder = Color.FromArgb("#808080");

            }
        }
        private async void AlterTile(Object o)
        {
            if (IsClicking)
            {
                try
                {
                    if (IsColoring)
                    {
                        Tile t = (Tile)o;
                        t.Blacken();
                        bool hasWon = true;
                        foreach (Tile T in Tiles)
                        {
                            if (T.CurrentColor != T.TrueColor) hasWon = false;
                        }
                        MarkRowColumn(t.X, t.Y);
                        if (hasWon)
                        {
                            timer.Stop();
                            GameWon();
                        }
                        if (!(t.X == SelectedX && t.Y == SelectedY))
                        {
                            Game.SelectTile(SelectedX, SelectedY, t.X, t.Y);
                            SelectedX = t.X;
                            SelectedY = t.Y;
                        }
                    }
                    else
                    {
                        Tile t = (Tile)o;
                        t.Mark();
                        if (!(t.X == SelectedX && t.Y == SelectedY))
                        {
                            Game.SelectTile(SelectedX, SelectedY, t.X, t.Y);
                            SelectedX = t.X;
                            SelectedY = t.Y;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                IsClicking = true;
                IsColoring = true;
                ColorBorder = Color.FromArgb("#ffbb00");
                MarkBorder = Color.FromArgb("#808080");
                MoveBorder = Color.FromArgb("#808080");

            }
        }
        private void MarkRowColumn(int selectedx, int selectedy)
        {
            ObservableCollection<Tile> joe = Tiles;
            bool rowcorrect = true;
            bool columncorrect = true;
            bool iscorrect = true;
            foreach (Tile T in Tiles)
            {
                if (T.X == selectedx && T.Y == selectedy && T.CurrentColor != T.TrueColor) iscorrect = false;
                if (T.X == selectedx && T.CurrentColor != T.TrueColor) columncorrect = false;
                if (T.Y == selectedy && T.CurrentColor != T.TrueColor) rowcorrect = false;
            }
            if (iscorrect)
            {
                if (columncorrect)
                {
                    foreach (Tile T in Tiles)
                    {
                        if (T.X == selectedx && T.CurrentColor != "Black") T.IsMarked = true;
                        Labels.Where(L => L.X == selectedx + 1).FirstOrDefault().TextColor = "Gray";
                    }
                }
                if (rowcorrect)
                {
                    foreach (Tile T in Tiles)
                    {
                        if (T.Y == selectedy && T.CurrentColor != "Black") T.IsMarked = true;
                        Labels.Where(L => L.Y == selectedy + 1).FirstOrDefault().TextColor = "Gray";
                    }
                }
            }
        }
        #endregion
        #region PostGame
        private async void GameWon()
        {
            Time--;
            //Open the leaderboard popup
            if (OpenPopup != null)
            {
                //await GetScoresByList();
                List<string> l = new List<string>();
                InitData();
                OpenPopup(l);
            }
            bool f = await SaveProgress(true);
        }
        private async void OnExit()
        {
            timer.Stop();
            bool f = await SaveProgress(false);
            // Navigate to the Register View page
            ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<LevelSelectPage>());
        }
        private async Task<bool> SaveProgress(bool haswon)
        {
            string layout = this.service.TileArrayToLayout(this.service.TileListToArray(Level.Size, Tiles));
            ScoreDTO score = new ScoreDTO(((App)Application.Current).LoggedInUser.Id, Level.LevelId, layout, Time+1, haswon);
            score = await this.service.AddScore(score);
            return true;
        }
        #endregion
    }
}
