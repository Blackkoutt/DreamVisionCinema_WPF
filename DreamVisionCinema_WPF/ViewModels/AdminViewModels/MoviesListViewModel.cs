using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Enums;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.Views.AdminViews;
using DreamVisionCinema_WPF.Views.Others;
using DreamVisionCinema_WPF_Logic.Exceptions;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using DreamVisionCinema_WPF_Logic.Model;
using System.Buffers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.AdminViewModels
{
    public class MoviesListViewModel : BaseViewModel
    {
        private static MoviesListViewModel _instance = null;

        private ObservableCollection<Movie> movieList;
        private string? searchValue;

        private IMovieRepository movieRepository;
        public ICommand EditMovieCommand { get; set; }
        public ICommand DeleteMovieCommand { get; set; }
        public ICommand SearchMovieListCommand { get; set; }

        public MoviesListViewModel(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
            movieList = new ObservableCollection<Movie>();
            EditMovieCommand = new RelayCommand(OpenEditMovieView);
            DeleteMovieCommand = new RelayCommand(OpenDeleteMovieView);
            SearchMovieListCommand = new RelayCommand(SearchMovieList);
            LoadOrRefreshMovieList();
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

        private void OpenDeleteMovieView(object parameter)
        {
            Movie selectedMovie = (Movie)parameter;

            DeleteMessage dialog = new DeleteMessage(selectedMovie);
            dialog.Show();
        }

        private void OpenEditMovieView(object parameter)
        {
            Movie selectedMovie = (Movie) parameter;

            EditMovie dialog = new EditMovie(selectedMovie);
            dialog.Show();
        }

        public void LoadOrRefreshMovieList()
        {
            try
            {
               MovieList = new ObservableCollection <Movie>( movieRepository.GetAllMovies());
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
        public static MoviesListViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = DIContainer.GetContainer().Resolve<MoviesListViewModel>();
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
