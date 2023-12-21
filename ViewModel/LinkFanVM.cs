using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Football_App.Model;
using Football_App.Service;
using Football_App.Utilities;

namespace Football_App.ViewModel
{
    public class LinkFanVM: ViewModelBase
    {
        private IWindowService _windowService;
        
        private readonly DBRepository _repo;
        private ObservableCollection<Club> _availableClubs;
        private Club _selectedAvailableClub;
        private Fan _selectedFan;
        public ICommand AddAvailableClubCommand { get; set; }

        public LinkFanVM(IWindowService windowService = null)
        {
            _windowService = windowService;
            _selectedFan = FanVM.FanVMVmStatic.SelectedFan;
            _repo = new DBRepository();
            
            AddAvailableClubCommand = new RelayCommand(param => AddAvailableClub());
        }
        
        
        public ObservableCollection<Club> AvailableClubs
        {
            get
            {
                _availableClubs = new ObservableCollection<Club>(_repo.GetAvailableClubs(SelectedFan));
                return _availableClubs;
            }
            set { _availableClubs = value; OnPropertyChanged(); }
        }
        
        // Связка выбранного ВОЗМОЖНОГО клуба из датагрид LinkFanWindow
        public Club SelectedAvailableClub
        {
            get { return _selectedAvailableClub; }
            set
            {
                _selectedAvailableClub = value;
            }
        }
        
        public Fan SelectedFan
        {
            get { return _selectedFan; }
            set
            {
                _selectedFan = value;
            }
        }
        
        private void AddAvailableClub()
        {
            // получаем выделенный объект
            Fan fan = SelectedFan;
            Club club = SelectedAvailableClub;
            
            // если ни одного объекта не выделено, выходим
            if (club is null)
            {
                MessageBox.Show("Сначала выделите клуб, который хотите добавить в избранное");
                return;
            }
            
            if (MessageBox.Show($"Вы точно хотите добавить клуб {club.ClubName} в избранное", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                   _repo.AddAvailableClubToFan(fan, club);
                    
                    OnPropertyChanged("AvailableClubs");
            
                    MessageBox.Show("Успешно добавили в избранное");
                }
                catch (Exception ex)
                {
                    // TO-DO: потом в логи!
                    MessageBox.Show(ex.Message.ToString());
                }
            
                _windowService.CloseEditFanClubWindow();
            }
        }
    }
}