using Football_App.ViewModel;
using System.Windows;
using Football_App.Service;

namespace Football_App.View
{
    public partial class PlayersListWindow : Window
    {
        public PlayersListWindow()
        {
            InitializeComponent();
            
            // Создаем экземпляр WindowService
            var windowService = new WindowService();

            // Передаем WindowService в viewmodel, для доступности текущего контекста в WindowService
            DataContext = new PlayerVM(windowService);
        }
    }
}
