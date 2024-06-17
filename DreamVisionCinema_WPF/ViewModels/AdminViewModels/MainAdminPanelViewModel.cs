using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.ViewModels.ClientViewModels;
using DreamVisionCinema_WPF_Logic.Model;
using System.Windows.Input;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.AdminViewModels
{
    public class MainAdminPanelViewModel : BaseViewModel
    {
        public ICommand HomeViewCommand { get; set; }
        public ICommand MovieListViewCommand { get; set; }
        public ICommand MovieDetailsViewCommand { get; set; }
        public ICommand ReservationListViewCommand { get; set; }
        public ICommand StatisticsPanelViewCommand { get; set; }
        public ICommand MostProfitableMoviesViewCommand { get; set; }
        public ICommand MostPopularMoviesViewCommand { get; set; }
        public ICommand DragMoveCommand => base.DragCommand;

        public MovieDetailsViewModel MovieDetailsVM { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public MoviesListViewModel MovieListVM { get; set; }
        public StatisticsPanelViewModel StatisticsPanelVM { get; set; }
        public ReservationListViewModel ReservationListViewVM { get; set; }
        public MostPopularMoviesViewModel MostPopularMoviesVM { get; set; }
        public MostProfitableMoviesViewModel MostProfitableMoviesVM { get; set; }

        private object _currentView;
        private static MainAdminPanelViewModel _instance = null;

        public MainAdminPanelViewModel()
        {
            HomeVM = new HomeViewModel();
            MovieListVM = DIContainer.GetContainer().Resolve<MoviesListViewModel>(); 
            //new MoviesListViewModel();
            DateTime time = DateTime.Now;
            Room room = new Room(69, 69);
            Movie movie = new Movie(69, "allah", time, 69, "69", room, "a", "a", "a");
            MovieDetailsVM = new MovieDetailsViewModel(movie);
            StatisticsPanelVM = new StatisticsPanelViewModel();
            ReservationListViewVM = DIContainer.GetContainer().Resolve<ReservationListViewModel>();
            MostProfitableMoviesVM = new MostProfitableMoviesViewModel();
            MostPopularMoviesVM = new MostPopularMoviesViewModel();

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

        public object CurentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public static MainAdminPanelViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainAdminPanelViewModel();
                    return _instance;
                }
                else
                    return _instance;
            }
        }
    }
}
