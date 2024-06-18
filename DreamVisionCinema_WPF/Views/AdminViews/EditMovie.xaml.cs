using DreamVisionCinema_WPF.ViewModels.AdminViewModels;
using DreamVisionCinema_WPF_Logic.Model;
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
    /// Logika interakcji dla klasy EditMovie.xaml
    /// </summary>
    public partial class EditMovie : Window
    {
        public EditMovie(Movie movie)
        {
            EditMovieViewModel EditMovieVM = EditMovieViewModel.Instance;
            EditMovieVM.Movie = movie;
            EditMovieVM.MovieId = movie.Id;
            EditMovieVM.Title = $"\"{movie.Title}\"";
            EditMovieVM.Date = movie.Date;
            EditMovieVM.Price = (int)movie.Price;
            EditMovieVM.RoomNumber = movie.Room.Number;
            EditMovieVM.Description = movie.Description;
            EditMovieVM.AgeCategory = int.Parse(movie.AgeCategory);
            string indirectPosterPath = "pack://application:,,,/Assets/Posters/";
            EditMovieVM.PathToPoster = movie.PathToPoster.Split(indirectPosterPath)[1];

            DataContext = EditMovieVM;
            InitializeComponent();
        }
    }
}
