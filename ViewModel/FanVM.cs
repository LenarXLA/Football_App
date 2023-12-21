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
    public class FanVM : ViewModelBase
    {
        private IWindowService _windowService;
        
        // публичная статичная переменная нужна для обновления списка из viewmodel AddClubWindow
        public static FanVM FanVMVmStatic;
        
        private readonly DBRepository _repo;
        private ObservableCollection<Fan> _fans;
        private ObservableCollection<Club> _fanClubs;
        
        private Fan _selectedFan;
        private Club _selectedFanClub;
        
        public ICommand ShowAddFanWinCommand { get; set; }
        public ICommand ShowFanClubsCommand { get; set; }
        public ICommand ShowAddClubToFanWinCommand { get; set; }
        public ICommand UnlinkClubCommand { get; set; }
        public ICommand DeleteFanCommand { get; set; }
        
        public FanVM(IWindowService windowService = null)
        {
            _windowService = windowService;
            _repo = new DBRepository();
            
            ShowAddFanWinCommand = new RelayCommand(param => ShowAddFanWin());
            ShowFanClubsCommand = new RelayCommand(param => ShowFanClubs());
            ShowAddClubToFanWinCommand = new RelayCommand(param => ShowAddClubToFanWin());
            UnlinkClubCommand = new RelayCommand(param => UnlinkClub());
            DeleteFanCommand = new RelayCommand(param => DeleteFan());
            
            // передаем контекст
            FanVMVmStatic = this;
        }
        
        // все футболисты
        public ObservableCollection<Fan> Fans
        {
            get
            {
                _fans = new ObservableCollection<Fan>(_repo.GetFans());
                return _fans;
            }
            set { _fans = value; OnPropertyChanged();}
        }
        
        private void ShowAddFanWin()
        {
            _windowService.OpenAddFanWindow();
        }
        
        public void UpdateFanList()
        {
            // чистим наш ObservableCollection - для обновления записей 
            if (_fans.Any())
            {
                _fans.Clear();
            }
            
            _fans = new ObservableCollection<Fan>(_repo.GetFans());
            OnPropertyChanged("Fans");
        }
        
        private void ShowFanClubs()
        {
            _windowService.OpenFanListWindow();
        }
        
        private void ShowAddClubToFanWin()
        {
            _windowService.OpenEditFanClubWindow();
        }
        
        // Связка выбранного болельщика из датагрид FanListWindow
        public Fan SelectedFan
        {
            get { return _selectedFan; }
            set
            {
                _selectedFan = value;
                OnPropertyChanged();
            }
        }
        
        // Связка выбранного болельщика - отображение соответствующих избранных клубов
        public ObservableCollection<Club> FanClubs
        {
            get
            {
                if (SelectedFan != null)
                {
                    _fanClubs = new ObservableCollection<Club>(_repo.GetClubsByFan(SelectedFan));
                }
                else
                {
                    _fanClubs = null;
                }
                
                return _fanClubs;
            }
            set 
            { 
                _fanClubs = value; 
                OnPropertyChanged();
            }
        }
        
        public void UpdateFanClubLists()
        {
            // чистим наш ObservableCollection - для обновления записей 
            if (_fanClubs.Any())
            {
                _fanClubs.Clear();
            }
            
            _fanClubs = new ObservableCollection<Club>(_repo.GetClubsByFan(SelectedFan));
            OnPropertyChanged("FanClubs");
        }
        
        
        // Связка выбранного клуба из датагрид FanListWindow
        public Club SelectedFanClub
        {
            get { return _selectedFanClub; }
            set
            {
                _selectedFanClub = value;
                OnPropertyChanged();
            }
        }
        
        
        private void UnlinkClub()
        {
            // получаем выделенный объект
            Fan fan = SelectedFan;
            Club club = SelectedFanClub;

            if (club is null)
            {
                MessageBox.Show("Сначала выделите клуб, которого хотите удалить из избранного");
                return;
            }

            if (MessageBox.Show($"Вы точно хотите удалить клуб {club.ClubName} из избранного", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _repo.UnlinkClubFromFan(fan, club);
                    
                    // обновляем список клубов - перегружая коллекцию
                    OnPropertyChanged("FanClubs");
                    
                    MessageBox.Show("Успешно убрали клуб из избранного!");
                }
                catch (Exception ex)
                {
                    // TO-DO: потом в логи!
                    MessageBox.Show(ex.Message.ToString());
                }
                
            }
        }
        
        private void DeleteFan()
        {
            // получаем выделенный объект
            Fan fan = SelectedFan;

            if (fan is null)
            {
                MessageBox.Show("Сначала выделите болельщика, которого хотите удалить");
                return;
            }

            if (MessageBox.Show($"Вы точно хотите удалить болельщика {fan.FanName}", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _repo.RemoveFan(fan);
                    
                    // обновляем список болельщиков - перегружая коллекцию
                    OnPropertyChanged("Fans");
                    
                    MessageBox.Show("Успешно удалили болельщика!");
                }
                catch (Exception ex)
                {
                    // TO-DO: потом в логи!
                    MessageBox.Show(ex.Message.ToString());
                }
                
            }
        }
        
    }
}
