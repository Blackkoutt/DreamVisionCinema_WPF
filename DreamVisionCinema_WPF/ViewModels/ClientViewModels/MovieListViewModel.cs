using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Enums;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.Views.AdminViews;
using DreamVisionCinema_WPF_Logic.Exceptions;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using DreamVisionCinema_WPF_Logic.Model;
using System.Buffers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.ClientViewModels
{
    public class MovieListViewModel : BaseViewModel
    {
        private IMovieRepository repository;
        private static MovieListViewModel _instance = null;
        public ObservableCollection<Movie> moviesList { get; set; }
        public ICommand MovieDetailsViewCommand { get; private set; }
        public ICommand SearchMovieListCommand { get; set; }

        private string? searchValue;

        public MovieListViewModel(IMovieRepository movieRepository)
        {
            repository = movieRepository;
            MoviesList = new ObservableCollection<Movie>();
            LoadOrRefreshMovieList();
            SearchMovieListCommand = new RelayCommand(SearchMovieList);
            MovieDetailsViewCommand = new RelayCommand(OpenReservationModal);
        }

        private void OpenReservationModal(object parameter)
        {
            if (parameter is Movie movie)
            {
                Console.WriteLine(movie.Title);
                GlobalEventAggregator.RaiseViewChanged(new MovieDetailsViewModel(movie), "Detale Filmu");
            }
        }
        public void LoadOrRefreshMovieList()
        {
            try
            {
                MoviesList = new ObservableCollection<Movie>(repository.GetAllMovies());
            }
            catch (MovieListIsEmptyException MLIEE)
            {
                MakeAlert(MLIEE.Message, AlertTypeEnum.Info, true);
            }
            catch (Exception ex)
            {
                MakeAlert(ex.Message, AlertTypeEnum.Info, true);
            }
        }
        private void SearchMovieList(object parameter)
        {
            string search_value = (SearchValue != null) ? SearchValue : string.Empty;

            if (MoviesList.Any())
            {
                try
                {
                    MoviesList = new ObservableCollection<Movie>(repository.FilterList(search_value));
                }
                catch (CannotFindMatchingMovieException CFMME)
                {
                    MakeAlert(CFMME.Message, AlertTypeEnum.Info, true);
                    return;
                }
                catch (Exception ex)
                {
                    MakeAlert(ex.Message, AlertTypeEnum.Error, true);
                    return;
                }
            }
            else
            {
                MakeAlert("Lista filmów jest pusta", AlertTypeEnum.Info, true);
            }
        }

        public static MovieListViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = DIContainer.GetContainer().Resolve<MovieListViewModel>();
                    return _instance;
                }
                else
                    return _instance;
            }
        }
        public ObservableCollection<Movie> MoviesList
        {
            get { return moviesList; }
            set
            {
                moviesList = value;
                OnPropertyChanged();
            }
        }
        public string? SearchValue
        {
            get { return searchValue; }
            set { searchValue = value; }
        }

    }
}
