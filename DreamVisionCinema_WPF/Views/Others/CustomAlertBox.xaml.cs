using DreamVisionCinema_WPF.Enums;
using DreamVisionCinema_WPF.ViewModels;
using DreamVisionCinema_WPF.ViewModels.Others;
using System.Windows;
using System.Windows.Media.Media3D;

namespace DreamVisionCinema_WPF.Views.Others
{
    /// <summary>
    /// Logika interakcji dla klasy CustomAlertBox.xaml
    /// </summary>
    public partial class CustomAlertBox : Window
    {
        
        public CustomAlertBox(string message, AlertTypeEnum type, bool isTimerEnabled)
        {       
            InitializeComponent();
            Left = SystemParameters.WorkArea.Width - Width - 5;
            Top = SystemParameters.WorkArea.Height - Height;

            DataContext = new CustomAlertBoxViewModel(message, type, isTimerEnabled, this.Width, this.Height, this);
        }
    }
}
