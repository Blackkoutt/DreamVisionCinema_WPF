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

namespace DreamVisionCinema_WPF.Views.AdminViews
{
    /// <summary>
    /// Logika interakcji dla klasy MainAdminPanel.xaml
    /// </summary>
    public partial class MainAdminPanel : Window
    {
        public MainAdminPanel()
        {
            InitializeComponent();
        }

        private void addMovieBtn_Click(object sender, RoutedEventArgs e)
        {
            AddMovie dialog = new AddMovie();
            dialog.Show();
        }
    }
}
