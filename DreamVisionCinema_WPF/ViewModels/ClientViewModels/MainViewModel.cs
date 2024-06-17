using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF_Logic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DreamVisionCinema_WPF.ViewModels.ClientViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand DragMoveCommand => base.DragCommand;
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand MovieListViewCommand { get; set; }
        public RelayCommand ClientReservationListViewCommand {  get; set; }

        public HomeViewModel HomeVM { get; set; }
        public MovieListViewModel MovieListVM { get; set; }
        public ClientReservationListViewModel ClientReservationListVM { get; set; }
        private object _currentView;

        private static MainViewModel _instance = null;

        private string _tabText;

        public static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainViewModel();                   
                    return _instance;
                }
                else
                    return _instance;
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

        public MainViewModel()
        {
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
    }
}
