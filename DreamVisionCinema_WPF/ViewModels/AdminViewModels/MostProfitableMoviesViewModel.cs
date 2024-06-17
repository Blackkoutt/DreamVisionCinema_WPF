using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF_Logic.Interfaces;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Windows.Input;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.AdminViewModels
{
    public class MostProfitableMoviesViewModel : BaseViewModel
    {
        private static MostProfitableMoviesViewModel _instance = null;
        private PlotModel? chartModel;
        public ICommand BackFromProfitableMoviesCommand { get; set; }

        public MostProfitableMoviesViewModel(IReservationRepository reservationRepository)
        {
            Dictionary<string, double> data = reservationRepository.GetMoviesIncome();
            PrepareChart(data);
            BackFromProfitableMoviesCommand = new RelayCommand(OpenMainStatisticPanel);
        }

        private void OpenMainStatisticPanel(object parameter)
        {
            MainAdminPanelViewModel.Instance.CurentView = StatisticsPanelViewModel.Instance;
        }

        private void PrepareChart(Dictionary<string, double> data)
        {
            var model = new PlotModel
            {
                Title = "Najbardziej dochodowe filmy",
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
                TextColor = OxyColors.White,

            };
            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                TicklineColor = OxyColors.White,
                FontSize = 14,
                TextColor = OxyColors.White,
                Title = "Dochód (zł)",
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

            ChartModel = model;
        }

        public static MostProfitableMoviesViewModel Instance
        {
            get
            {
                return DIContainer.GetContainer().Resolve<MostProfitableMoviesViewModel>();
                /*if (_instance == null)
                {
                    _instance = DIContainer.GetContainer().Resolve<MostProfitableMoviesViewModel>();
                    return _instance;
                }
                else
                    return _instance;*/
            }
        }
        public PlotModel? ChartModel
        {
            get { return chartModel; }
            set
            {
                chartModel = value;
                OnPropertyChanged();
            }
        }
    }
}
