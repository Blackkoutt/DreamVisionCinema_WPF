using System.Windows.Controls;
using DreamVisionCinema_WPF_Logic.Model;
using DreamVisionCinema_WPF.ViewModels.ClientViewModels;

namespace DreamVisionCinema_WPF.Views.ClientViews
{
    public partial class MovieDetailsView : UserControl
    {
        public MovieDetailsView()
        {
            InitializeComponent();
        }

        public MovieDetailsView(Movie movie) : this() // Wywołanie domyślnego konstruktora
        {
            DataContext = new MovieDetailsViewModel(movie);
        }
    }
}
