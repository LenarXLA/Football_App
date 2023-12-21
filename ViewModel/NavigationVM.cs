using System.Windows;
using Football_App.Utilities;
using System.Windows.Input;

namespace Football_App.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand ClubsCommand { get; set; }
        public ICommand PlayersCommand { get; set; }
        public ICommand FansCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Club(object obj) => CurrentView = new ClubVM();
        private void Player(object obj) => CurrentView = new PlayerVM();
        private void Fan(object obj) => CurrentView = new FanVM();

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            ClubsCommand = new RelayCommand(Club);
            PlayersCommand = new RelayCommand(Player);
            FansCommand = new RelayCommand(Fan);

            // Стартовая страница!
            CurrentView = new HomeVM();
        }
    }
}
