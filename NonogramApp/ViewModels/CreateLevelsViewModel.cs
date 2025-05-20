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
            SaveCommand = new Command(OnExit);
            ColorCommand = new Command(ColorTile);
            UploadCommand = new Command(OnUpload);
            AltColorTileCommand = new Command((Object o) => AltColorTile(o));
            RestartCommand = new Command(Restart);
            BoardSize = 5;
            Sizes = new List<string>()
            {
                "5x5",
                "10x10",
                "15x15",
                "20x20",
                "25x25"
            };
            Title = "";
            SelectedX = 0;
            SelectedY = 0;
            SelectedSize = "5x5";
            TitleError = "Title required";
            CreateGame();
        }
        #region (Instance)Variables
        public ICommand AltColorTileCommand { get; private set; }
        public ICommand ColorCommand { get; set; }
        public ICommand UpCommand { get; set; }
        public ICommand DownCommand { get; set; }
        public ICommand LeftCommand { get; set; }
        public ICommand RightCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand UploadCommand { get; set; }
        public ICommand RestartCommand { get; set; }
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
        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                ValidateTitleError();
                OnPropertyChanged(nameof(Title));
            }
        }
        private List<string> sizes;
        public List<string> Sizes
        {
            get
            {
                return sizes;
            }
            set
            {
                sizes = value;
                OnPropertyChanged(nameof(Sizes));
            }
        }
        private string selectedSize;
        public string SelectedSize
        {
            get
            {
                return selectedSize;
            }
            set
            {
                selectedSize = value;
                ExtractSizeFromSizes(SelectedSize);
                ExpandGrid(BoardSize);
                CreateGame();
                OnPropertyChanged(nameof(SelectedSize));
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
                OnPropertyChanged(nameof(BoardSize));
            }
        }
        private string titleError;
        public string TitleError
        {
            get
            {
                return titleError;
            }
            set
            {
                titleError = value;
                OnPropertyChanged(nameof(TitleError));
            }
        }

        private bool showTitleError;
        public bool ShowTitleError
        {
            get
            {
                return showTitleError;
            }
            set
            {
                showTitleError = value;
                OnPropertyChanged(nameof(ShowTitleError));
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
        public void ExpandGrid(int size)
        {
            Rows = new();
            Columns = new();
            for (int i = 0; i < size; i++)
            {
                Rows.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
                Columns.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            }
        }
        public void ExtractSizeFromSizes(string size)
        {
            BoardSize = int.Parse((size.Substring(0, ((size.Length - 1) / 2))));
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
        public async void ValidateTitleError()
        {
            this.ShowTitleError = string.IsNullOrEmpty(Title);
        }
        private void Up()
        {
            int temp = SelectedY;
            SelectedY -= 1;
            if (SelectedY < 0) SelectedY = BoardSize - 1;
            Game.SelectTile(SelectedX, temp, SelectedX, SelectedY);
        }
        private void Down()
        {
            int temp = SelectedY;
            SelectedY += 1;
            if (SelectedY > BoardSize - 1) SelectedY = 0;
            Game.SelectTile(SelectedX, temp, SelectedX, SelectedY);
        }
        private void Left()
        {
            int temp = SelectedX;
            SelectedX -= 1;
            if (SelectedX < 0) SelectedX = BoardSize - 1;
            Game.SelectTile(temp, SelectedY, SelectedX, SelectedY);
        }
        private void Right()
        {
            int temp = SelectedX;
            SelectedX += 1;
            if (SelectedX > BoardSize - 1) SelectedX = 0;
            Game.SelectTile(temp, SelectedY, SelectedX, SelectedY);
        }
        private void ColorTile()
        {
            Tiles.Where(T => T.X == SelectedX && T.Y == SelectedY).FirstOrDefault().Blacken();
        }
        private void Restart()
        {
            Game.ClearBoard();
        }
        private async void AltColorTile(Object o)
        {
            try
            {
                Tile t = (Tile)o;
                t.Blacken();
                if (!(t.X == SelectedX && t.Y == SelectedY))
                {
                    Game.SelectTile(SelectedX, SelectedY, t.X, t.Y);
                    SelectedX = t.X; 
                    SelectedY = t.Y;
                }
            }
            catch(Exception ex)
            {

            }
        }
        #endregion
        #region PostGame
        private async void OnUpload()
        {
            bool f = await SaveProgress();
            ((App)Application.Current).MainPage.Navigation.PopAsync();
        }
        private async void OnExit()
        {
            ((App)Application.Current).MainPage.Navigation.PopAsync();
        }
        private async Task<bool> SaveProgress()
        {
            ValidateTitleError();
            if (!ShowTitleError)
            {
                string layout = this.service.TileArrayToLayout(this.service.TileListToArray(BoardSize, Tiles));
                this.service.AddLevel(new LevelDTO(0, Title, layout, BoardSize, ((App)Application.Current).LoggedInUser.Id, 1));
                await Application.Current.MainPage.DisplayAlert("Creation Successful!", "Level created and is pending approval.", "Ok");
                foreach (Tile T in Tiles)
                {
                    T.CurrentColor = "White";
                }
                Title = "";
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
