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
using CommunityToolkit.Maui.Core.Extensions;

namespace NonogramApp.ViewModels
{
    public class ApprovePuzzlesViewModel : ViewModelBase
    {
        private NonogramService service;
        private readonly IServiceProvider serviceProvider;
        public ApprovePuzzlesViewModel(NonogramService service, IServiceProvider serviceProvider)
        {
            this.service = service;
            this.serviceProvider = serviceProvider;
            VisLevels = new ObservableCollection<LevelWithMakerName>();
            ApproveCommand = new Command(Approve);
            DeclineCommand = new Command(Decline);
            InitData();
        }
        public async void InitData()
        {
            await SetLevels();
            await SetPlayers();
            await SetLevelsWithMakerNames();
        }
        public ICommand ApproveCommand { get; set; }
        public ICommand DeclineCommand { get; set; }
        private ObservableCollection<LevelDTO> levels;
        public ObservableCollection<LevelDTO> Levels
        {
            get => levels;
            set
            {
                levels = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<LevelWithMakerName> visLevels;
        public ObservableCollection<LevelWithMakerName> VisLevels
        {
            get => visLevels;
            set
            {
                visLevels = value;
                OnPropertyChanged();
            }
        }
        private List<PlayerDTO> players;
        public List<PlayerDTO> Players
        {
            get => players;
            set
            {
                players = value;
                OnPropertyChanged();
            }
        }
        public async Task SetLevels()
        {
            List<LevelDTO> templevels = await service.GetPendingLevels();
            Levels = new ObservableCollection<LevelDTO>();
            foreach (LevelDTO l in templevels)
            {
                Levels.Add(l);
            }
            ObservableCollection<LevelDTO> orderlvels = Levels.OrderBy(l => l.Size).ToObservableCollection();
            Levels = orderlvels.ToObservableCollection();
        }
        public async Task SetPlayers()
        {
            List<PlayerDTO> tempplayers = await service.GetPendingLevelMakers();
            Players = new();
            if (tempplayers != null)
            {
                foreach (PlayerDTO p in tempplayers)
                {
                    Players.Add(p);
                }
            }
        }
        public async Task SetLevelsWithMakerNames()
        {
            VisLevels = new();
            foreach (LevelDTO l in Levels)
            {
                VisLevels.Add(new LevelWithMakerName(l, Players.Where(p => p.Id == l.CreatorId).FirstOrDefault()));
            }
        }
        public async void Approve()
        {

        }
        public async void Decline()
        {

        }
    }
}
