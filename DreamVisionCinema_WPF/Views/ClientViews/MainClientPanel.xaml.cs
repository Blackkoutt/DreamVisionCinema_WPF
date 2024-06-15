using DreamVisionCinema_WPF.ViewModels.ClientViewModels;
using System.Windows;

namespace DreamVisionCinema_WPF.Views.ClientViews
{
    /// <summary>
    /// Interaction logic for MainClientPanel.xaml
    /// </summary>
    public partial class MainClientPanel : Window
    {
        public MainClientPanel()
        {
             DataContext = MainViewModel.Instance;
             InitializeComponent();
        }
    }
}
