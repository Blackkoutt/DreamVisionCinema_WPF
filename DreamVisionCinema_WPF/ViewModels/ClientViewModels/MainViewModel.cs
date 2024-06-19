using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.ViewModels.AdminViewModels;
using DreamVisionCinema_WPF_Logic.Interfaces;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using DreamVisionCinema_WPF_Logic.Model;
using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.ClientViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand HomeViewCommand { get; set; }
        public ICommand MovieListViewCommand { get; set; }
        public ICommand ClientReservationListViewCommand { get; set; }
        public ICommand BackFromClientViewCommand { get; set; }
        public ICommand DragMoveCommand => base.DragCommand;
        public HomeViewModel HomeVM { get; set; }
        public MovieListViewModel MovieListVM { get; set; }
        public ClientReservationListViewModel ClientReservationListVM { get; set; }

        private object _currentView;
        private string? topBarIcon;
        private string? topBarText;
        private Brush? iconColor;
        private string? movieListButtonDirection;
        private string? reservationButtonDirection;
        private Thickness? movieListThickness;
        private Thickness? reservationThickness;
        private Brush? movieListBrush;
        private Brush? reservationBrush;

        private IReservationRepository reservationRepository;
        private IMovieRepository movieRepository;

        private static MainViewModel _instance = null;

        public MainViewModel(IReservationRepository reservationRepository, IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
            this.reservationRepository = reservationRepository;

            HomeVM = new HomeViewModel();
            MovieListVM = DIContainer.GetContainer().Resolve<MovieListViewModel>();
            ClientReservationListVM = new ClientReservationListViewModel();

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
            ClientReservationListViewCommand = new RelayCommand(o =>
            {
                SetDefault();
                TopBarText = "Lista rezerwacji";
                TopBarIcon = "Solid_BookOpen";
                IconColor = (Brush)Application.Current.Resources["reservation_color"];
                ReservationButtonDirection = "RightToLeft";
                ReservationThickness = new Thickness(8, 0, 0, 0);
                ReservationBrush = (Brush)Application.Current.Resources["reservation_color"];
                CurentView = ClientReservationListVM;
            });
            BackFromClientViewCommand = new RelayCommand(OpenSelectionView);
            GlobalEventAggregator.ViewChanged += OnViewChanged;
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
            MovieListThickness = defaultTickness;
            MovieListButtonDirection = "LeftToRight";
            ReservationButtonDirection = "LeftToRight";
            MovieListBrush = Brushes.White;
            ReservationBrush = Brushes.White;
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

        private void OnViewChanged(object sender, ViewChangedEventArgs e)
        {
            TopBarText = e.TabText;
            CurentView = e.NewView;
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
        public string? ReservationButtonDirection
        {
            get { return reservationButtonDirection; }
            set
            {
                reservationButtonDirection = value;
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
        public Thickness? ReservationThickness
        {
            get { return reservationThickness; }
            set
            {
                reservationThickness = value;
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
        public Brush? ReservationBrush
        {
            get { return reservationBrush; }
            set
            {
                reservationBrush = value;
                OnPropertyChanged();
            }
        }
        public static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = DIContainer.GetContainer().Resolve<MainViewModel>();
                    return _instance;
                }
                else
                    return _instance;
            }
        }
    }
}
