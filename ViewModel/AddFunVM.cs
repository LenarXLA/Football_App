using System;
using System.Windows;
using System.Windows.Input;
using Football_App.Model;
using Football_App.Service;
using Football_App.Utilities;

namespace Football_App.ViewModel
{
    public class AddFunVM: ViewModelBase
    {
        private IWindowService _windowService;
        
        private readonly DBRepository _repo;
        private string _fanName;
        public ICommand AddFanCommand { get; set; }
        
        public AddFunVM(IWindowService windowService = null)
        {
            _windowService = windowService;
            _repo = new DBRepository();
            
            AddFanCommand = new RelayCommand(param => AddFan());
        }
        
        private void AddFan()
        {
            if (string.IsNullOrEmpty(FanName))
            {
                MessageBox.Show("Заполните имя болельщика!");
                return;
            }
            
            Fan fan = new Fan
            {
                FanName = FanName
            };

            try
            {
                _repo.AddFan(fan);
            }
            catch (Exception ex)
            {
                // в логи потом
                MessageBox.Show(ex.Message.ToString());
            }
            
            _windowService.CloseAddFanWindow();
            MessageBox.Show("Успешное добавление болельщика!");
        }
        
        public string FanName
        {
            get { return _fanName; }
            set { _fanName = value; OnPropertyChanged(); }
        }
    }
}