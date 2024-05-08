using DreamVisionCinema_WPF.Observable;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamVisionCinema_WPF.Views.ClientViews.ViewModel
{
    class MainViewModel : ObservableObject
    {

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand MovieListViewCommand { get; set; }
        public RelayCommand MovieDetailsViewCommand { get; set; }

        public MovieDetailsViewModel MovieDetailsVM { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public MovieListViewModel MovieListVM { get; set; }
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

        public MainViewModel() {
            HomeVM = new HomeViewModel();
            MovieListVM = new MovieListViewModel();
            MovieDetailsVM = new MovieDetailsViewModel();
            _currentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurentView = HomeVM;
            });

            MovieListViewCommand = new RelayCommand(o =>
            {
                CurentView = MovieListVM;
            });

            MovieDetailsViewCommand = new RelayCommand(o =>
            {
                CurentView = MovieDetailsVM;
            });
        }

    }
}
