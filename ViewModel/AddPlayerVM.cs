using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using Football_App.Model;
using Football_App.Service;
using Football_App.Utilities;

namespace Football_App.ViewModel
{
    public class AddPlayerVM: ViewModelBase
    {
        private IWindowService _windowService;
        
        private readonly DBRepository _repo;
        private string _playerName;
        private string _playerBirthday;
        private string _playerSNILS;
        public ICommand AddPlayerCommand { get; set; }
        
        public AddPlayerVM(IWindowService windowService = null)
        {
            _windowService = windowService;
            _repo = new DBRepository();
            
            AddPlayerCommand = new RelayCommand(param => AddPlayer());
        }
        
        private void AddPlayer()
        {
            if (string.IsNullOrEmpty(PlayerName))
            {
                MessageBox.Show("Заполните ФИО игрока!");
                return;
            }
            
            if (string.IsNullOrEmpty(PlayerSNILS))
            {
                MessageBox.Show("Заполните СНИЛС!");
                return;
            }
            
            // Преобразуем дату к локальному региональному стандарту
            var date = "";
            if (PlayerBirthday != null)
            {
                var dt = DateTime.ParseExact(PlayerBirthday, "MM/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                date = dt.ToString("dd/MM/yyyy");
            }
            
            FootballPlayer player = new FootballPlayer
            {
                PlayerName = PlayerName,
                PlayerBirthdate = date,
                PlayerSNILS = PlayerSNILS
            };

            try
            {
                _repo.AddPlayer(player);
            }
            catch (Exception ex)
            {
                // в логи потом
                MessageBox.Show(ex.Message.ToString());
            }
            
            _windowService.CloseAddFootballPlayerWindow();
            MessageBox.Show("Успешное добавление игрока!");
        }
        
        public string PlayerName
        {
            get { return _playerName; }
            set { _playerName = value; OnPropertyChanged(); }
        }
        
        public string PlayerBirthday
        {
            get { return _playerBirthday; }
            set { _playerBirthday = value; OnPropertyChanged(); }
        }
        
        public string PlayerSNILS
        {
            get { return _playerSNILS; }
            set { _playerSNILS = value; OnPropertyChanged(); }
        }
    }
}