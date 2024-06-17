using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Enums;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF_Logic.Exceptions;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.AdminViewModels
{
    public class AddMovieViewModel : BaseViewModel
    {   
        private string? title;
        private DateTime date;
        private int price;
        private int duration;
        private int roomNumber;
        private IMovieRepository movieRepository;
        public ICommand DragMoveCommand => base.DragCommand;
        public ICommand AddMovieCommand { get; set; }

        public AddMovieViewModel(IMovieRepository movieRepository) 
        {
            this.movieRepository = movieRepository;
            AddMovieCommand = new RelayCommand(AddMovie);
            Date = DateTime.Now.AddDays(1);
        }

        private void AddMovie(object parameter)
        {
            string dateString = Date.ToString("dd/MM/yyyy HH:mm");
            string priceString = Price.ToString();
            string durationString = $"{Duration / 60}:{(Duration % 60).ToString("D2")}";
            string roomNumberString = RoomNumber.ToString();    
            try
            {
                movieRepository.AddMovie(null, Title, dateString, priceString, durationString, roomNumberString);   // Wywołanie metody dodającej film
            }
            catch (CannotConvertException CCE)
            {
                MakeAlert(CCE.Message, AlertTypeEnum.Error, true);
                return;
            }
            catch (IncorrectParametrException IPE)
            {
                MakeAlert(IPE.Message, AlertTypeEnum.Error, true);
                return;
            }
            catch (NoRoomWithGivenNumberException NRWGNE)
            {
                MakeAlert(NRWGNE.Message, AlertTypeEnum.Error, true);
                return;
            }
            catch (MoviesCollisionException MCE)
            {
                MakeAlert(MCE.Message, AlertTypeEnum.Error, true);
                return;
            }
            catch (Exception ex)
            {
                MakeAlert(ex.Message, AlertTypeEnum.Error, true);
                return;
            }

            MakeAlert("Poprawnie dodano film do listy", AlertTypeEnum.Success, true);
            MoviesListViewModel.Instance.LoadOrRefreshMovieList();

            if (parameter is Window window)
            {
                window.Close();
            }
        }

        public static AddMovieViewModel Instance
        {
            get
            {
                return DIContainer.GetContainer().Resolve<AddMovieViewModel>();
            }
        }

        public int RoomNumber
        {
            get { return roomNumber; }
            set
            {
                roomNumber = value;
                OnPropertyChanged();
            }
        }


        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged();
            }
        }


        public int Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged();
            }
        }


        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }


        public string? Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }
    }
}
