using System.Windows;
using System.Windows.Controls;

namespace Football_App.Utilities
{
    // Кастомная кнопка для переходов боковой панели
    public class Btn : RadioButton
    {
        static Btn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Btn), new FrameworkPropertyMetadata(typeof(Btn)));
        }
    }
}
