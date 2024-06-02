using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF_Logic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DreamVisionCinema_WPF.Views.ClientViews.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        BaseViewModel _baseViewModel = new BaseViewModel();

        public ICommand MinimizeCommand => _baseViewModel.MinimizeCommand;
        public ICommand MaximizeCommand => _baseViewModel.MaximizeCommand;
        public ICommand CloseCommand => _baseViewModel.CloseCommand;
        public ICommand DragMoveCommand => _baseViewModel.DragCommand;

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand MovieListViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public MovieListViewModel MovieListVM { get; set; }
        private object _currentView;

        private static MainViewModel _instance = null;

        public static MainViewModel Instance
        {
            get { 
                if(_instance == null)
                {
                    _instance = new MainViewModel();
                    return _instance;
                }else    
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

        public MainViewModel() {
            HomeVM = new HomeViewModel();
            MovieListVM = new MovieListViewModel(this);
            _currentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurentView = HomeVM;
            });

            MovieListViewCommand = new RelayCommand(o =>
            {
                CurentView = MovieListVM;
            });

        }

    }
}
