using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.ViewModels.ClientViewModels;
using DreamVisionCinema_WPF.Views.AdminViews;
using DreamVisionCinema_WPF_Logic.Interfaces;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using DreamVisionCinema_WPF_Logic.Model;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.AdminViewModels
{
    public class MainAdminPanelViewModel : BaseViewModel
    {
        public ICommand HomeViewCommand { get; set; }
        public ICommand MovieListViewCommand { get; set; }
        public ICommand ReservationListViewCommand { get; set; }
        public ICommand StatisticsPanelViewCommand { get; set; }
        public ICommand AddMovieCommand { get; set; }
        public ICommand BackFromAdminViewCommand { get; set; }
        public ICommand DragMoveCommand => base.DragCommand;
        public HomeViewModel HomeVM { get; set; }
        public MoviesListViewModel MovieListVM { get; set; }
        public StatisticsPanelViewModel StatisticsPanelVM { get; set; }
        public ReservationListViewModel ReservationListViewVM { get; set; }

        private object _currentView;
        private string? topBarIcon;
        private string? topBarText;
        private Brush? iconColor;
        private string? movieListButtonDirection;
        private string? addMovieButtonDirection;
        private string? reservationButtonDirection;
        private string? statisitcsButtonDirection;
        private Thickness? movieListThickness;
        private Thickness? addMovieThickness;
        private Thickness? reservationThickness;
        private Thickness? statisticsThickness;
        private Brush? movieListBrush;
        private Brush? addMovieBrush;
        private Brush? reservationBrush;
        private Brush? statisticsBrush;
        private IMovieRepository movieRepository;
        private IReservationRepository reservationRepository;

        private static MainAdminPanelViewModel _instance = null;

        public MainAdminPanelViewModel(IMovieRepository movieRepository, IReservationRepository reservationRepository)
        {
            this.movieRepository = movieRepository;
            this.reservationRepository = reservationRepository;

            HomeVM = new HomeViewModel();
            MovieListVM = DIContainer.GetContainer().Resolve<MoviesListViewModel>(); 
            StatisticsPanelVM = DIContainer.GetContainer().Resolve<StatisticsPanelViewModel>();
            ReservationListViewVM = DIContainer.GetContainer().Resolve<ReservationListViewModel>();

            TopBarText = "Strona główna";
            TopBarIcon = "Solid_Home";
            IconColor = (Brush)Application.Current.Resources["purple_1"];
            SetDefault();

            _currentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                TopBarText = "Strona główna";
                TopBarIcon = "Solid_Home";
                IconColor = (Brush)Application.Current.Resources["purple_1"];
                CurentView = HomeVM;
            });

            AddMovieCommand = new RelayCommand(o =>
            {
                SetDefault();
                AddMovieButtonDirection = "RightToLeft";
                AddMovieThickness = new Thickness(8, 0, 0, 0);
                AddMovieBrush = (Brush)Application.Current.Resources["add_movie_color"];
                AddMovie dialog = new AddMovie();
                dialog.Show();
            });

            MovieListViewCommand = new RelayCommand(o =>
            {
                SetDefault();
                TopBarText = "Lista filmów";
                TopBarIcon = "Solid_Film";
                IconColor = (Brush)Application.Current.Resources["purple_1"];
                MovieListButtonDirection = "RightToLeft";
                MovieListThickness = new Thickness(8, 0, 0, 0);
                MovieListBrush = (Brush)Application.Current.Resources["purple_1"];
                CurentView = MovieListVM;
            });

            StatisticsPanelViewCommand = new RelayCommand(o =>
            {
                SetDefault();
                TopBarText = "Panel statystyk";
                TopBarIcon = "Regular_ChartBar";
                IconColor = (Brush)Application.Current.Resources["statistics_color"];
                StatisitcsButtonDirection = "RightToLeft";
                StatisticsThickness = new Thickness(8, 0, 0, 0);
                StatisticsBrush = (Brush)Application.Current.Resources["statistics_color"];
                CurentView = StatisticsPanelVM;
            });

            ReservationListViewCommand = new RelayCommand(o =>
            {
                SetDefault();
                TopBarText = "Lista rezerwacji";
                TopBarIcon = "Solid_BookOpen";
                IconColor = (Brush)Application.Current.Resources["reservation_color"];
                ReservationButtonDirection = "RightToLeft";
                ReservationThickness = new Thickness(8, 0, 0, 0);
                ReservationBrush = (Brush)Application.Current.Resources["reservation_color"];
                CurentView = ReservationListViewVM;
            });
            BackFromAdminViewCommand = new RelayCommand(OpenSelectionView);
        }
        
        private void OpenSelectionView(object parameter)
        {
            MainWindow mainWindow = new MainWindow();

            mainWindow.Show();

            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private void SetDefault()
        {
            var defaultTickness = new Thickness(0, 0, 0, 0);
            ReservationThickness = defaultTickness;
            AddMovieThickness = defaultTickness;
            StatisticsThickness = defaultTickness;
            MovieListThickness = defaultTickness;
            MovieListButtonDirection = "LeftToRight";
            AddMovieButtonDirection = "LeftToRight";
            ReservationButtonDirection = "LeftToRight";
            StatisitcsButtonDirection = "LeftToRight";
            MovieListBrush = Brushes.White;
            AddMovieBrush = Brushes.White;
            ReservationBrush = Brushes.White;
            StatisticsBrush = Brushes.White;
        }

        protected override void CloseWindow(object parameter)
        {
            movieRepository.SaveMoviesToFile();
            reservationRepository.SaveReservationsToFile();

            if (parameter is Window window)
            {
                window.Close();
            }
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
        public string? TopBarText
        {
            get { return topBarText; }
            set 
            {
                topBarText = value;
                OnPropertyChanged();
            }
        }
        public Brush? IconColor
        {
            get { return iconColor; }
            set 
            {
                iconColor = value;
                OnPropertyChanged();
            }
        }


        public string? TopBarIcon
        {
            get { return topBarIcon; }
            set 
            {
                topBarIcon = value;
                OnPropertyChanged();
            }
        }
        public string? StatisitcsButtonDirection
        {
            get { return statisitcsButtonDirection; }
            set
            {
                statisitcsButtonDirection = value;
                OnPropertyChanged();
            }
        }
        public string? ReservationButtonDirection
        {
            get { return reservationButtonDirection; }
            set
            {
                reservationButtonDirection = value;
                OnPropertyChanged();
            }
        }
        public string? AddMovieButtonDirection
        {
            get { return addMovieButtonDirection; }
            set
            {
                addMovieButtonDirection = value;
                OnPropertyChanged();
            }
        }
        public string? MovieListButtonDirection
        {
            get { return movieListButtonDirection; }
            set
            {
                movieListButtonDirection = value;
                OnPropertyChanged();
            }
        }
        public Thickness? MovieListThickness
        {
            get { return movieListThickness; }
            set
            {
                movieListThickness = value;
                OnPropertyChanged();
            }
        }
        public Thickness? AddMovieThickness
        {
            get { return addMovieThickness; }
            set
            {
                addMovieThickness = value;
                OnPropertyChanged();
            }
        }
        public Thickness? ReservationThickness
        {
            get { return reservationThickness; }
            set
            {
                reservationThickness = value;
                OnPropertyChanged();
            }
        }
        public Thickness? StatisticsThickness
        {
            get { return statisticsThickness; }
            set
            {
                statisticsThickness = value;
                OnPropertyChanged();
            }
        }
        public Brush? MovieListBrush
        {
            get { return movieListBrush; }
            set
            {
                movieListBrush = value;
                OnPropertyChanged();
            }
        }
        public Brush? AddMovieBrush
        {
            get { return addMovieBrush; }
            set
            {
                addMovieBrush = value;
                OnPropertyChanged();
            }
        }
        public Brush? ReservationBrush
        {
            get { return reservationBrush; }
            set
            {
                reservationBrush = value;
                OnPropertyChanged();
            }
        }
        public Brush? StatisticsBrush
        {
            get { return statisticsBrush; }
            set
            {
                statisticsBrush = value;
                OnPropertyChanged();
            }
        }

        public static MainAdminPanelViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = DIContainer.GetContainer().Resolve<MainAdminPanelViewModel>();
                    return _instance;
                }
                else
                    return _instance;
            }
        }
    }
}
