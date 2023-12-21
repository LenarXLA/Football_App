using System.Windows;

namespace Football_App.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // ограничение уменьшения окна
            MinWidth = 600;
            MinHeight = 400;
        }
    }
}
