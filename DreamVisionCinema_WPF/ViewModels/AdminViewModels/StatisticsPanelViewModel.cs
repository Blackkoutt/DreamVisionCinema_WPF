using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.ViewModels;
using DreamVisionCinema_WPF.Views.AdminViews;
using DreamVisionCinema_WPF_Logic.Interfaces;
using System.Windows.Input;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.AdminViewModels
{
    public class StatisticsPanelViewModel : BaseViewModel
    {
        private int totalIncome;
        private int incomeGuideline;
        private int incomePercent;
        public ICommand MostPopularMoviesViewCommand { get; set; }
        public ICommand MostProfitableMoviesViewCommand { get; set; }
        public StatisticsPanelViewModel(IReservationRepository reservationRepository)
        {
            TotalIncome = (int)reservationRepository.GetTotalIncome();
            IncomeGuideline = 10000;
            IncomePercent = CalculateIncomePercent();
            MostPopularMoviesViewCommand = new RelayCommand(OpenMostPopularMoviesView);
            MostProfitableMoviesViewCommand = new RelayCommand(OpenMostProfitableMoviesView);
        }

        private void OpenMostProfitableMoviesView(object parameter)
        {
            MainAdminPanelViewModel.Instance.CurentView = MostProfitableMoviesViewModel.Instance;
        }

        private void OpenMostPopularMoviesView(object parameter)
        {
            MainAdminPanelViewModel.Instance.CurentView = MostPopularMoviesViewModel.Instance;
        }

        private int CalculateIncomePercent()
        {
            int percentage = (int)(((double)totalIncome / (double)IncomeGuideline) * 100);
            return percentage;
        }
        public static StatisticsPanelViewModel Instance
        {
            get
            {
                return DIContainer.GetContainer().Resolve<StatisticsPanelViewModel>();
            }
        }
        public int TotalIncome
        {
            get { return totalIncome; }
            set
            {
                totalIncome = value;
                OnPropertyChanged();
                IncomePercent = CalculateIncomePercent(); // Update IncomePercent when TotalIncome changes
            }
        }

        public int IncomeGuideline
        {
            get { return incomeGuideline; }
            set
            {
                incomeGuideline = value;
                OnPropertyChanged();
                IncomePercent = CalculateIncomePercent(); // Update IncomePercent when IncomeGuideline changes
            }
        }

        public int IncomePercent
        {
            get { return incomePercent; }
            set
            {
                incomePercent = value;
                OnPropertyChanged();
            }
        }
    }

}
