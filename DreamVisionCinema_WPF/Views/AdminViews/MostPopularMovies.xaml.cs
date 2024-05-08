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

namespace DreamVisionCinema_WPF.Views.AdminViews
{
    /// <summary>
    /// Logika interakcji dla klasy MostPopularMovies.xaml
    /// </summary>
    public partial class MostPopularMovies : UserControl
    {
        public MostPopularMovies()
        {
            InitializeComponent();
            Dictionary<string, int> data = new Dictionary<string, int>();
            data.Add("Film1", 100);
            data.Add("Film2", 200);
            data.Add("Film3", 300);
            data.Add("Film4", 400);
            data.Add("Film5", 500);
            data.Add("Film6", 600);
            data.Add("Film7", 700);
            data.Add("Film8", 800);
            data.Add("Film9", 900);
            data.Add("Film10", 1000);

            PrepareChart(data);
        }
        private void PrepareChart(Dictionary<string, int> data)
        {
            var model = new PlotModel
            {
                Title = "Najpopularniejsze filmy",
                TextColor = OxyColors.White,
                PlotAreaBorderColor = OxyColors.White
            };

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Title = "Tytuł filmu",
                TitleFontWeight = OxyPlot.FontWeights.Bold,
                TitleFontSize = 16,
                FontSize = 14,
                TicklineColor = OxyColors.White,
                TextColor = OxyColors.White
            };

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColors.White,
                FontSize = 14,
                TextColor = OxyColors.White,
                Title = "Ilość zarezerwowanych miejsc",
                TitleFontWeight = OxyPlot.FontWeights.Bold,
                TitleFontSize = 16,
                MajorGridlineStyle = LineStyle.Dash,
                MajorGridlineColor = OxyColors.White
            };

            var barSeries = new BarSeries
            {
                FillColor = OxyColor.FromRgb(7, 146, 232)
            };

            foreach (var item in data)
            {
                categoryAxis.Labels.Add(item.Key);
                barSeries.Items.Add(new BarItem(item.Value));
            }

            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            model.Series.Add(barSeries);

            popularChart.Model = model;
        }
    }
}
