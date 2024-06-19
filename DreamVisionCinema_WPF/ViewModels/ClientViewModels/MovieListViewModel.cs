using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Enums;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.Views.ClientViews;
using DreamVisionCinema_WPF_Logic.Exceptions;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using DreamVisionCinema_WPF_Logic.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.ClientViewModels
{
    public class MovieListViewModel : BaseViewModel
    {
        private static MovieListViewModel _instance = null;

        private ObservableCollection<Movie> movieList;
        private string? searchValue;

        private IMovieRepository movieRepository;
        public ICommand MovieDetailsViewCommand { get; set; }
        public ICommand SearchMovieListCommand { get; set; }

        public MovieListViewModel(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
            movieList = new ObservableCollection<Movie>();
            SearchMovieListCommand = new RelayCommand(SearchMovieList);
            MovieDetailsViewCommand = new RelayCommand(OpenReservationModal);
            LoadOrRefreshMovieList();
        }
        private void OpenReservationModal(object parameter)
        {
            if (parameter is Movie movie)
            {
                GlobalEventAggregator.RaiseViewChanged(new MovieDetailsViewModel(movie), "Detale Filmu");
            }
        }

        private void SearchMovieList(object parameter)
        {
            string search_value = (SearchValue != null) ? SearchValue : string.Empty;

            if (MovieList.Any())
            {
                try
                {
                    MovieList = new ObservableCollection<Movie>(movieRepository.FilterList(search_value));
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
        public void LoadOrRefreshMovieList()
        {
            try
            {
                MovieList = new ObservableCollection<Movie>(movieRepository.GetAllMovies());
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
        public ObservableCollection<Movie> MovieList
        {
            get { return movieList; }
            set
            {
                movieList = value;
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
