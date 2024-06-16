using DreamVisionCinema_WPF.ViewModels.AdminViewModels;
using System.Windows.Controls;

namespace DreamVisionCinema_WPF.Views.AdminViews
{
    /// <summary>
    /// Logika interakcji dla klasy MoviesList.xaml
    /// </summary>
    public partial class MoviesList : UserControl
    { 
        public MoviesList()
        {
            DataContext = MoviesListViewModel.Instance;
            InitializeComponent();
        }
    }
}
