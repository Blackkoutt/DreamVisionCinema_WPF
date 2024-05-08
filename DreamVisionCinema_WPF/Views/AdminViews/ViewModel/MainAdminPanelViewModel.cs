using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.Views.ClientViews.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamVisionCinema_WPF.Views.AdminViews.ViewModel
{
    class MainAdminPanelViewModel : ObservableObject
    {

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand MovieListViewCommand { get; set; }
        public RelayCommand MovieDetailsViewCommand { get; set; }
        public RelayCommand ReservationListViewCommand { get; set; }
        public RelayCommand StatisticsPanelViewCommand { get; set; }
        public RelayCommand MostProfitableMoviesViewCommand { get; set; }
        public RelayCommand MostPopularMoviesViewCommand { get; set; }

        public MovieDetailsViewModel MovieDetailsVM { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public MoviesListViewModel MovieListVM { get; set; }
        public StatisticsPanelViewModel StatisticsPanelVM { get; set; }
        public ReservationListViewModel ReservationListViewVM { get; set; }

        public MostPopularMovies MostPopularMoviesVM { get; set; }
        public MostProfitableMovies MostProfitableMoviesVM { get; set; }

        private object _currentView;

        public object CurentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainAdminPanelViewModel()
        {
            HomeVM = new HomeViewModel();
            MovieListVM = new MoviesListViewModel();
            MovieDetailsVM = new MovieDetailsViewModel();
            StatisticsPanelVM = new StatisticsPanelViewModel();
            ReservationListViewVM = new ReservationListViewModel();
            MostProfitableMoviesVM = new MostProfitableMovies();
            MostPopularMoviesVM = new MostPopularMovies();

            _currentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurentView = HomeVM;
            });

            MovieListViewCommand = new RelayCommand(o =>
            {
                CurentView = MovieListVM;
            });

            StatisticsPanelViewCommand = new RelayCommand(o =>
            {
                CurentView = StatisticsPanelVM;
            });

            MovieDetailsViewCommand = new RelayCommand(o =>
            {
                CurentView = MovieDetailsVM;
            });


            ReservationListViewCommand = new RelayCommand(o =>
            {
                CurentView = ReservationListViewVM;
            });

            MostPopularMoviesViewCommand = new RelayCommand(o =>
            {
                CurentView = MostPopularMoviesVM;
            });
            MostProfitableMoviesViewCommand = new RelayCommand(o =>
            {
                CurentView = MostProfitableMoviesVM;
            });
        }
    }
}
