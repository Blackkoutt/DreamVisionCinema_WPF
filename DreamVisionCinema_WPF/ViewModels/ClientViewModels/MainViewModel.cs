using DreamVisionCinema_WPF.Observable;
using System.Windows.Input;

namespace DreamVisionCinema_WPF.ViewModels.ClientViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand DragMoveCommand => base.DragCommand;

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand MovieListViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public MovieListViewModel MovieListVM { get; set; }
        private object _currentView;

        private static MainViewModel _instance = null;

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

        public MainViewModel()
        {
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
