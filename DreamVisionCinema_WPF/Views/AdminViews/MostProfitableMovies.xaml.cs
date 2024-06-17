using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DreamVisionCinema_WPF.ViewModels.AdminViewModels;

namespace DreamVisionCinema_WPF.Views.AdminViews
{
    /// <summary>
    /// Logika interakcji dla klasy MostProfitableMovies.xaml
    /// </summary>
    public partial class MostProfitableMovies : UserControl
    {
        public MostProfitableMovies()
        {
            DataContext = MostProfitableMoviesViewModel.Instance;
            InitializeComponent();
        }
    }
}
