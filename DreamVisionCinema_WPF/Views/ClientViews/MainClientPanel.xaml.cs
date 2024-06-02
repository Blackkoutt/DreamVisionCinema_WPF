using DreamVisionCinema_WPF.Views.ClientViews.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DreamVisionCinema_WPF.Views.ClientViews
{
    /// <summary>
    /// Interaction logic for MainClientPanel.xaml
    /// </summary>
    public partial class MainClientPanel : Window
    {
        public MainClientPanel()
        {
            this.DataContext = MainViewModel.Instance;
             InitializeComponent();
        }
    }
}
