using Football_App.Service;
using Football_App.ViewModel;
using System.Windows;


namespace Football_App.View
{
    public partial class LinkPlayerWindow : Window
    {
        public LinkPlayerWindow()
        {
            InitializeComponent();
            
            // Создаем экземпляр WindowService
            var windowService = new WindowService();

            // Передаем WindowService в viewmodel, для доступности текущего контекста в WindowService
            DataContext = new LinkPlayerVM(windowService);
        }
    }
}
