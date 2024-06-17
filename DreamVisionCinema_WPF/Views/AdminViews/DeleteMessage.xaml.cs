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
    /// Logika interakcji dla klasy DeleteMessage.xaml
    /// </summary>
    public partial class DeleteMessage : Window
    {
        public DeleteMessage(Movie movie)
        {
            DeleteMessageViewModel DeleteMovieVM = DeleteMessageViewModel.Instance;
            DeleteMovieVM.MovieId = movie.Id;
            DeleteMovieVM.Title = $"\"{movie.Title}\"";

            DataContext = DeleteMovieVM;
            InitializeComponent();
        }
    }
}
