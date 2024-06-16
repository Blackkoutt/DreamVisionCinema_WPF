using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Enums;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF_Logic.Exceptions;
using DreamVisionCinema_WPF_Logic.Interfaces;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.AdminViewModels
{
    public class DeleteMessageViewModel : BaseViewModel
    {   
        public ICommand CancelDeleteCommand { get; set; }
        public ICommand ConfirmDeleteCommand { get; set; }
        public ICommand DragMoveCommand => base.DragCommand;

        private string? title;
        private int movieId;
        private IReservationRepository reservationRepository;
        public DeleteMessageViewModel(IReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;
            CancelDeleteCommand = new RelayCommand(CloseDeleteWindow);
            ConfirmDeleteCommand = new RelayCommand(DeleteMovie);
        }

        private void DeleteMovie(object parameter)
        {
            string id = MovieId.ToString();
            try
            {
                reservationRepository.RemoveTicketForReservatedMovies(id);
                reservationRepository.RemoveMovieWithReservation(id);   
            }
            catch (CannotConvertException CCE)
            {
                MakeAlert(CCE.Message, AlertTypeEnum.Error, true);
                return;
            }
            catch (NoMovieWithGivenIdException NMWGIE)
            {
                MakeAlert(NMWGIE.Message, AlertTypeEnum.Error, true);
                return;
            }
            catch (NoReservationWithGivenIdException NRWGIE)
            {
                MakeAlert(NRWGIE.Message, AlertTypeEnum.Error, true);
                return;
            }
            catch (CannotDestroyTicketException CDTE)
            {
                MakeAlert(CDTE.Message, AlertTypeEnum.Error, true);
                return;
            }
            catch (Exception ex)
            {
                MakeAlert(ex.Message, AlertTypeEnum.Error, true);
                return;
            }
            MakeAlert("Pomyślnie usunięto film z listy", AlertTypeEnum.Success, true);
            MoviesListViewModel.Instance.LoadOrRefreshMovieList();
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private void CloseDeleteWindow(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        public static DeleteMessageViewModel Instance
        {
            get
            {
                return DIContainer.GetContainer().Resolve<DeleteMessageViewModel>();
            }
        }
        public string? Title
        {
            get { return title; }
            set { title = value; }
        }
        public int MovieId
        {
            get {return movieId;  }
            set { movieId = value; }
        }
    }
}
