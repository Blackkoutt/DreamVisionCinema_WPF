using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF_Logic.Interfaces;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using DreamVisionCinema_WPF_Logic.Model;
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
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.ClientViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand DragMoveCommand => base.DragCommand;
        public ICommand HomeViewCommand { get; set; }
        public ICommand MovieListViewCommand { get; set; }
        public ICommand ClientReservationListViewCommand {  get; set; }
        public HomeViewModel HomeVM { get; set; }
        public MovieListViewModel MovieListVM { get; set; }
        public ClientReservationListViewModel ClientReservationListVM { get; set; }
        private object _currentView;

        private static MainViewModel _instance = null;

        private string _tabText;
        private IReservationRepository reservationRepository;
        private IMovieRepository movieRepository;

        private MainViewModel(IReservationRepository reservationRepository, IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
            this.reservationRepository = reservationRepository;

            HomeVM = new HomeViewModel();
            MovieListVM = new MovieListViewModel();
            ClientReservationListVM = new ClientReservationListViewModel();
            _currentView = HomeVM;
            tabText = "Strona Główna";

            HomeViewCommand = new RelayCommand(o =>
            {
                CurentView = HomeVM;
                tabText = "Strona Główna";
            });

            MovieListViewCommand = new RelayCommand(o =>
            {
                CurentView = MovieListVM;
                tabText = "Lista Filmów";
            });
            ClientReservationListViewCommand = new RelayCommand(o =>
            {
                CurentView = ClientReservationListVM;
                tabText = "Rezerwacje klienta";
            });
            GlobalEventAggregator.ViewChanged += OnViewChanged;
        }
         private void OnViewChanged(object sender, ViewChangedEventArgs e)
        {
            tabText = e.TabText;
            CurentView = e.NewView;
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
        public string tabText
        {
            get { return _tabText; }
            set
            {
                _tabText = value;
                OnPropertyChanged();
            }
        }
    }
}
