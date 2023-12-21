using Football_App.Model;
using Football_App.Service;
using Football_App.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Football_App.ViewModel
{
    public class ClubVM : ViewModelBase
    {
        private IWindowService _windowService;
        
        // публичная статичная переменная нужна для обновления списка клубов из viewmodel AddClubWindow
        public static ClubVM ClubVmStatic;
        
        private readonly DBRepository _repo;
        private ObservableCollection<Club> _clubs;
        private Club _selectedClub;
        
        private object _lockMutex = new object();
        
        public ICommand ShowAddClubWinCommand { get; set; }
        public ICommand DeleteClubCommand { get; set; }
        public ICommand ShowClubPlayersCommand { get; set; }
        
        public ClubVM(IWindowService windowService = null)
        {
            _windowService = windowService;
            _repo = new DBRepository();
            _clubs = new ObservableCollection<Club>();

            // Передача связыванию что данные будут грузиться асинхронно
            BindingOperations.EnableCollectionSynchronization(Clubs, _lockMutex);

            // Загрузка данных в датагрид
            LoadData();
            
            ShowAddClubWinCommand = new RelayCommand(param => ShowAddClubWin());
            DeleteClubCommand = new RelayCommand(param => DeleteClub());
            ShowClubPlayersCommand = new RelayCommand(param => ShowClubPlayers());
            
            // передаем контекст
            ClubVmStatic = this;
        }
        
        public ObservableCollection<Club> Clubs
        {
            get { return _clubs; }
            set { _clubs = value; OnPropertyChanged(); }
        }
        
        // Связка выбранного клуба из датагрид
        public Club SelectedClub
        {
            get { return _selectedClub; }
            set
            {
                _selectedClub = value;
                OnPropertyChanged("FootballPlayers");
                OnPropertyChanged("ClubName");
            }
        }

        private void ShowAddClubWin()
        {
            _windowService.OpenAddClubWindow();
        }
        
        private void DeleteClub()
        {
            // получаем выделенный объект
            Club club = SelectedClub;

            // если ни одного объекта не выделено, выходим
            if (club is null)
            {
                MessageBox.Show("Сначала выделите клуб, который хотите удалить");
                return;
            }

            if (MessageBox.Show($"Вы точно хотите удалить клуб {SelectedClub.ClubName}", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _repo.RemoveClub(SelectedClub);
                    
                    // обновляем список клубов - перегружая коллекцию
                    LoadData();
                
                    MessageBox.Show("Успешное удаление!");
                }
                catch (Exception ex)
                {
                    // TO-DO: потом в логи!
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        
        private void ShowClubPlayers()
        {
            _windowService.OpenPlayersListWindow();
        }

        #region Helper

        // Загружаем клубы асинхронно, чтобы не блокировать интерфейс
        public void LoadData()
        {
            _ = LoadDataAsync();
        }

        private Task LoadDataAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                // чистим наш ObservableCollection - для обновления записей 
                if (_clubs.Any())
                {
                    _clubs.Clear();
                }
                //Загрузка клубов и передача в коллекцию ObservableCollection
                foreach (Club c in _repo.GetClubs())
                {
                    _clubs.Add(c);
                }
            });
        }

        #endregion
    }
}
