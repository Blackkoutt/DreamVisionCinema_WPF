using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.ViewModels.ClientViewModels;
using System.Windows.Controls;
using Unity;

namespace DreamVisionCinema_WPF.Views.ClientViews
{
    /// <summary>
    /// Interaction logic for MovieListView.xaml
    /// </summary>
    public partial class MovieListView : UserControl
    {
        public MovieListView()
        {
            DataContext = DIContainer.GetContainer().Resolve<MovieListViewModel>();
            InitializeComponent();
        }
    }
}
