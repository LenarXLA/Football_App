using System;
using Football_App.Model;
using Football_App.Service;
using Football_App.Utilities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Football_App.ViewModel
{
    public class PlayerVM : ViewModelBase
    {
        private IWindowService _windowService;
        
        // публичная статичная переменная нужна для обновления списка клубов из viewmodel AddClubWindow
        public static PlayerVM PlayerVmStatic;
        
        private readonly DBRepository _repo;
        private ObservableCollection<FootballPlayer> _players;
        private ObservableCollection<FootballPlayer> _allPlayers;
        private Club _selectedClub;
        private FootballPlayer _selectedPlayer;
        
        public ICommand EliminatePlayerCommand { get; set; }
        public ICommand ShowAddPlayerToClubWinCommand { get; set; }
        public ICommand ShowAddFootballPlayerWinCommand { get; set; }
        public ICommand DeleteFootballPlayerCommand { get; set; }
        public ICommand EditPlayerClubCommand { get; set; }
        
        public PlayerVM(IWindowService windowService = null)
        {
            _windowService = windowService;
            if (ClubVM.ClubVmStatic != null)
            {
                _selectedClub = ClubVM.ClubVmStatic.SelectedClub;
            }
            
            _repo = new DBRepository();
            
            EliminatePlayerCommand = new RelayCommand(param => EliminatePlayer());
            ShowAddPlayerToClubWinCommand = new RelayCommand(param => ShowAddPlayerToClubWin());
            ShowAddFootballPlayerWinCommand = new RelayCommand(param => ShowAddFootballPlayerWin());
            DeleteFootballPlayerCommand = new RelayCommand(param => DeleteFootballPlayer());
            EditPlayerClubCommand = new RelayCommand(param => EditPlayerClub());
            
            // передаем контекст
            PlayerVmStatic = this;
        }
        
        // Связка выбранного клуба из датагрид
        public Club SelectedClub
        {
            get { return _selectedClub; }
            set
            {
                _selectedClub = value;
                OnPropertyChanged("FootballPlayers");
            }
        }

        // Связка выбранного клуба - отображение соответствующих футболистов
        public ObservableCollection<FootballPlayer> FootballPlayers
        {
            get
            {
                if (SelectedClub != null)
                {
                    _players = new ObservableCollection<FootballPlayer>(_repo.GetPlayersByClubId(SelectedClub));
                }
                else
                {
                    _players = null;
                }
                
                return _players;
            }
            set { _players = value; OnPropertyChanged();}
        }
        
        // все футболисты
        public ObservableCollection<FootballPlayer> AllFootballPlayers
        {
            get
            {
                _allPlayers = new ObservableCollection<FootballPlayer>(_repo.GetPlayers());
                return _allPlayers;
            }
            set { _allPlayers = value; OnPropertyChanged();}
        }
        
        // Связка выбранного игрока из датагрид PlayerListWindow
        public FootballPlayer SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged();
            }
        }

        public void UpdatePlayerLists()
        {
            // чистим наш ObservableCollection - для обновления записей 
            if (_players.Any())
            {
                _players.Clear();
            }
            
            _players = new ObservableCollection<FootballPlayer>(_repo.GetPlayersByClubId(SelectedClub));
            OnPropertyChanged("FootballPlayers");
        }
        
        public void UpdateAllPlayerLists()
        {
            // чистим наш ObservableCollection - для обновления записей 
            if (_allPlayers.Any())
            {
                _allPlayers.Clear();
            }
            
            _allPlayers = new ObservableCollection<FootballPlayer>(_repo.GetPlayers());
            OnPropertyChanged("AllFootballPlayers");
        }
        
        private void EliminatePlayer()
        {
            // получаем выделенный объект
            FootballPlayer player = SelectedPlayer;

            if (player is null)
            {
                return;
            }

            if (MessageBox.Show($"Вы точно хотите исключить игрока {player.PlayerName}", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _repo.EliminatePlayerFromClub(player);
                    
                    // обновляем список игроков - перегружая коллекцию
                    OnPropertyChanged("FootballPlayers");
                    OnPropertyChanged("FreeFootballPlayers");
                    
                    MessageBox.Show("Успешно исключили игрока!");
                }
                catch (Exception ex)
                {
                    // TO-DO: потом в логи!
                    MessageBox.Show(ex.Message.ToString());
                }
                
            }
        }
        
        private void ShowAddPlayerToClubWin()
        {
            _windowService.OpenAddPlayerToClubWindow();
        }
        
        private void ShowAddFootballPlayerWin()
        {
            _windowService.OpenAddFootballPlayerWindow();
        }
        
        private void DeleteFootballPlayer()
        {
            // получаем выделенный объект
            FootballPlayer player = SelectedPlayer;

            if (player is null)
            {
                MessageBox.Show("Сначала выделите футболиста, которого хотите удалить");
                return;
            }

            if (MessageBox.Show($"Вы точно хотите удалить футболиста {player.PlayerName}", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _repo.RemovePlayer(player);
                    
                    // обновляем список игроков - перегружая коллекцию
                    OnPropertyChanged("AllFootballPlayers");
                    
                    MessageBox.Show("Успешно удалили футболиста!");
                }
                catch (Exception ex)
                {
                    // TO-DO: потом в логи!
                    MessageBox.Show(ex.Message.ToString());
                }
                
            }
        }
        
        private void EditPlayerClub()
        {
            _windowService.OpenEditPlayerClubWindow();
        }
        
    }
}
