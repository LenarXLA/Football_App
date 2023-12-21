using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Football_App.Model;
using Football_App.Service;
using Football_App.Utilities;

namespace Football_App.ViewModel
{
    public class LinkPlayerVM: ViewModelBase
    {
        private IWindowService _windowService;
        
        private readonly DBRepository _repo;
        private ObservableCollection<FootballPlayer> _freePlayers;
        private FootballPlayer _selectedFreePlayer;
        private Club _selectedClub;
        public ICommand AddFreePlayerToClubCommand { get; set; }

        public LinkPlayerVM(IWindowService windowService = null)
        {
            _windowService = windowService;
            _selectedClub = PlayerVM.PlayerVmStatic.SelectedClub;
            _repo = new DBRepository();
            
            AddFreePlayerToClubCommand = new RelayCommand(param => AddFreePlayerToClub());
        }
        
        
        public ObservableCollection<FootballPlayer> FreeFootballPlayers
        {
            get
            {
                _freePlayers = new ObservableCollection<FootballPlayer>(_repo.GetFreePlayers());
                return _freePlayers;
            }
            set { _freePlayers = value; OnPropertyChanged(); }
        }
        
        // Связка выбранного СВОБОДНОГО игрока из датагрид LinkPlayerWindow
        public FootballPlayer SelectedFreePlayer
        {
            get { return _selectedFreePlayer; }
            set
            {
                _selectedFreePlayer = value;
            }
        }
        
        public Club SelectedClub
        {
            get { return _selectedClub; }
            set
            {
                _selectedClub = value;
            }
        }
        
        private void AddFreePlayerToClub()
        {
            // получаем выделенный объект
            FootballPlayer player = SelectedFreePlayer;
            
            // если ни одного объекта не выделено, выходим
            if (player is null)
            {
                MessageBox.Show("Сначала выделите игрока, которого хотите привязать к клубу");
                return;
            }
            
            // здесь берем АКТУАЛЬНЫЙ клуб
            var currentClub = _repo.GetClubById(SelectedClub.ClubId);
            
            if (MessageBox.Show($"Вы точно хотите добавить игрока {player.PlayerName} в клуб {currentClub.ClubName}", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _repo.AddPlayerToClub(player, currentClub);
                    
                    OnPropertyChanged("FreeFootballPlayers");
            
                    MessageBox.Show("Успешно прикрепили игрока к клубу");
                }
                catch (Exception ex)
                {
                    // TO-DO: потом в логи!
                    MessageBox.Show(ex.Message.ToString());
                }
            
                _windowService.CloseAddPlayerToClubWindow();
            }
        }
    }
}