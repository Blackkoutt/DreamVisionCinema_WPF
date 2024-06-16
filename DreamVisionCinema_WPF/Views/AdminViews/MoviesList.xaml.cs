using DreamVisionCinema_WPF_Logic.Enums;
using DreamVisionCinema_WPF_Logic.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DreamVisionCinema_WPF.Views.AdminViews
{
    /// <summary>
    /// Logika interakcji dla klasy MoviesList.xaml
    /// </summary>
    public partial class MoviesList : UserControl
    { 
        public MoviesList()
        {
            InitializeComponent();

            List<Movie> movies = new List<Movie>();
            movies.Add(new Movie(1, "Najnowszy film bardzo dobry", DateTime.Now, 20.99, "1:30h", new Room(1, 100), "a", "a","a"));
            movies.Add(new Movie(2, "Film 2", DateTime.Now, 20.99, "1:30h", new Room(1, 100), "a", "a", "a"));

            moviesList.ItemsSource = movies;

          
        }

        private void editMovieBtn_Click(object sender, RoutedEventArgs e)
        {
            EditMovie dialog = new EditMovie();
            dialog.Show();
        }

        private void deleteMovieBtn_Click(object sender, RoutedEventArgs e)
        {
            DeleteMessage dialog = new DeleteMessage();
            dialog.Show();
        }
    }
}
