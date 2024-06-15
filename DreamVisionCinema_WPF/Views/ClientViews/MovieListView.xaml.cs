using DreamVisionCinema_WPF.ViewModels.ClientViewModels;
using System.Windows.Controls;

namespace DreamVisionCinema_WPF.Views.ClientViews
{
    /// <summary>
    /// Interaction logic for MovieListView.xaml
    /// </summary>
    public partial class MovieListView : UserControl
    {
        public MovieListView()
        {
            DataContext = new MovieListViewModel(MainViewModel.Instance);
            InitializeComponent();
        }
    }
}
