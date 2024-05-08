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
    /// Logika interakcji dla klasy AdminAuthentication.xaml
    /// </summary>
    public partial class AdminAuthentication : Window
    {
        public AdminAuthentication()
        {
            InitializeComponent();
        }

        private void signInBtn_Click(object sender, RoutedEventArgs e)
        {
            MainAdminPanel adminPanel = new MainAdminPanel();

            // Otwarcie nowego okna
            adminPanel.Show();

            // Zamknięcie bieżącego okna
            this.Close();
        }
    }
}
