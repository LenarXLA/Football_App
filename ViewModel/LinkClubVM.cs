using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Football_App.Model;
using Football_App.Service;
using Football_App.Utilities;

namespace Football_App.ViewModel
{
    public class LinkClubVM : ViewModelBase
    {
        private IWindowService _windowService;
        
        private readonly DBRepository _repo;
        private ObservableCollection<Club> _clubs;

        private FootballPlayer _selectedPlayer;
        private Club _selectedPlayerClub;
        public ICommand ChangePlayerClubCommand { get; set; }
        public ICommand ExcludePlayerCommand { get; set; }

        public LinkClubVM(IWindowService windowService = null)
        {
            _windowService = windowService;
            _repo = new DBRepository();
            
            // здесь вычисляем все клубы и среди них клуб выбранного игрока
            _clubs = new ObservableCollection<Club>(_repo.GetClubs());
            _selectedPlayer = PlayerVM.PlayerVmStatic.SelectedPlayer;
            if (SelectedPlayer.ClubId.HasValue)
            {
                _selectedPlayerClub = Clubs.FirstOrDefault(c => c.ClubId == SelectedPlayer.ClubId);
            }
            
            ChangePlayerClubCommand = new RelayCommand(param => ChangePlayerClub());
            ExcludePlayerCommand = new RelayCommand(param => ExcludePlayer());
        }
        
        public ObservableCollection<Club> Clubs
        {
            get
            {
                return _clubs;
            }
            set 
            { 
                _clubs = value; 
                OnPropertyChanged(); 
            }
        }
        
        public FootballPlayer SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged();
            }
        }
        
        public Club SelectedPlayerClub
        {
            get
            {
                return _selectedPlayerClub; 
            }
            set
            {
                _selectedPlayerClub = value;
                OnPropertyChanged();
            }
        }
        
        private void ChangePlayerClub()
        {
            // получаем выделенный объект
            FootballPlayer player = SelectedPlayer;
            Club club = SelectedPlayerClub;

            if (club == null)
            {
                MessageBox.Show("Выберите клуб пожалуйста");
                return;
            }        
            
            if (MessageBox.Show($"Вы точно хотите добавить игрока {player.PlayerName} в клуб {club.ClubName}", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _repo.ChangePlayerClub(player, club);
                    
                    OnPropertyChanged("AllFootballPlayers");
            
                    MessageBox.Show("Успешно прикрепили игрока к клубу");
                }
                catch (Exception ex)
                {
                    // TO-DO: потом в логи!
                    MessageBox.Show(ex.Message.ToString());
                }
            
                _windowService.CloseEditPlayerClubWindow();
            }
        }

        private void ExcludePlayer()
        {
            FootballPlayer player = SelectedPlayer;
            Club club = SelectedPlayerClub;
            
            if (club == null)
            {
                MessageBox.Show("Футболист уже не состоит ни в одном клубе");
                return;
            }      
            
            if (MessageBox.Show($"Вы точно хотите исключить игрока {player.PlayerName} из всех клубов", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _repo.EliminatePlayerFromClub(player);
                    
                    OnPropertyChanged("AllFootballPlayers");
            
                    MessageBox.Show("Успешно исключили игрока");
                }
                catch (Exception ex)
                {
                    // TO-DO: потом в логи!
                    MessageBox.Show(ex.Message.ToString());
                }
            
                _windowService.CloseEditPlayerClubWindow();
            }
        }
    }
}