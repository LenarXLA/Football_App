using System.Windows;
using Football_App.ViewModel;

namespace Football_App.View
{
    public partial class FanListWindow : Window
    {
        public FanListWindow()
        {
            InitializeComponent();
            
            // Передаем контекст предыдущего окна, так как работам с одним и тем же экземпяром viewmodel - FanVM
            DataContext = FanVM.FanVMVmStatic;
        }
    }
}