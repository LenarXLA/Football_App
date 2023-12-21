using System.Windows.Controls;
using Football_App.Service;
using Football_App.ViewModel;

namespace Football_App.View
{
    public partial class Fans : UserControl
    {
        public Fans()
        {
            InitializeComponent();
            
            // Создаем экземпляр WindowService
            var windowService = new WindowService();

            // Передаем WindowService в viewmodel, для доступности текущего контекста в WindowService
            DataContext = new FanVM(windowService);
        }
    }
}
