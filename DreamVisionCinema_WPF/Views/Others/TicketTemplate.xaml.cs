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

namespace DreamVisionCinema_WPF.Views.Others
{
    /// <summary>
    /// Interaction logic for TicketTemplate.xaml
    /// </summary>
    public partial class TicketTemplate : UserControl
    {
        public TicketTemplate()
        {
            InitializeComponent();
        }
        public void SetTicketData(string movieTitle, string seats, string date, string time)
        {
            MovieTitleTextBlock.Text = movieTitle;
            SeatsTextBlock.Text = seats;
            DateTextBlock.Text = date;
            TimeTextBlock.Text = time;
        }
    }
}
