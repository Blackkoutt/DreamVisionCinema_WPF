using System.Windows.Controls;
using DreamVisionCinema_WPF.ViewModels.AdminViewModels;

namespace DreamVisionCinema_WPF.Views.AdminViews
{
    /// <summary>
    /// Logika interakcji dla klasy MostPopularMovies.xaml
    /// </summary>
    public partial class MostPopularMovies : UserControl
    {
        public MostPopularMovies()
        {
            DataContext = MostPopularMoviesViewModel.Instance;
            InitializeComponent();
        }
    }
}
