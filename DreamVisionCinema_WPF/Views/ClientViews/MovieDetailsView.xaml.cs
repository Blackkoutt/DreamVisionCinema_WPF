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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DreamVisionCinema_WPF.Views.ClientViews
{
    /// <summary>
    /// Logika interakcji dla klasy MovieDetailsView.xaml
    /// </summary>
    public partial class MovieDetailsView : UserControl
    {
        public MovieDetailsView()
        {
            InitializeComponent();
        }

        private void OpenSeatChoice(object sender, RoutedEventArgs e)
        {
            SeatReservationWindow dialog = new SeatReservationWindow();
            dialog.Show();
        }
    }
}
