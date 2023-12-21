using System;
using System.Windows;
using System.Windows.Input;
using Football_App.Model;
using Football_App.Service;
using Football_App.Utilities;

namespace Football_App.ViewModel
{
    public class AddClubVM: ViewModelBase
    {
        private IWindowService _windowService;
        
        private readonly DBRepository _repo;
        private string _addedClubName;
        private string _addedClubCity;
        public ICommand AddClubCommand { get; set; }
        
        public AddClubVM(IWindowService windowService = null)
        {
            _windowService = windowService;
            _repo = new DBRepository();
            
            AddClubCommand = new RelayCommand(param => AddClub());
        }
        
        private void AddClub()
        {
            if (string.IsNullOrEmpty(AddedClubName))
            {
                MessageBox.Show("Заполните название клуба!");
                return;
            }
            
            if (string.IsNullOrEmpty(AddedClubCity))
            {
                MessageBox.Show("Заполните город клуба!");
                return;
            }
            
            Club club = new Club
            {
                ClubName = AddedClubName,
                ClubCity = AddedClubCity,
            };

            try
            {
                _repo.AddClub(club);
            }
            catch (Exception ex)
            {
                // в логи потом
                MessageBox.Show(ex.Message.ToString());
            }
            
            _windowService.CloseAddClubWindow();
            MessageBox.Show("Успешное добавление клуба!");
        }
        
        public string AddedClubName
        {
            get { return _addedClubName; }
            set { _addedClubName = value; OnPropertyChanged(); }
        }
        
        public string AddedClubCity
        {
            get { return _addedClubCity; }
            set { _addedClubCity = value; OnPropertyChanged(); }
        }
        
    }
}