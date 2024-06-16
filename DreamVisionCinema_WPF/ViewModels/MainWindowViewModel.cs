using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Enums;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.Views.AdminViews;
using DreamVisionCinema_WPF.Views.ClientViews;
using DreamVisionCinema_WPF_Logic.Exceptions;
using DreamVisionCinema_WPF_Logic.Interfaces;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private static MainWindowViewModel _instance = null;
        public ICommand DragMoveCommand => base.DragCommand;
        public ICommand OpenClientViewCommand { get; set; }
        public ICommand OpenAdminAuthenticationViewCommand { get; set; }
        private IMovieRepository movieRepository;
        private IReservationRepository reservationRepository;
        public MainWindowViewModel(IMovieRepository movieRepository, IReservationRepository reservationRepository)
        {
            this.reservationRepository = reservationRepository;
            this.movieRepository = movieRepository;
            LoadMovieAndRepositoryData();

            OpenClientViewCommand = new RelayCommand(OpenClientView);
            OpenAdminAuthenticationViewCommand = new RelayCommand(OpenAdminAuthenticationView);
        }

        private void LoadMovieAndRepositoryData()
        {
            try
            {
                movieRepository.ReadMoviesFromFile();
                reservationRepository.ReadReservationsFromFile();
            }
            catch (CannotReadFileException CRFE)
            {
                MakeAlert(CRFE.Message, AlertTypeEnum.Error, false);
            }
            catch (FileSyntaxException FSE)
            {
                MakeAlert(FSE.Message, AlertTypeEnum.Error, false);
            }
            catch (Exception ex)
            {
                MakeAlert(ex.Message, AlertTypeEnum.Error, false);
            }
        }

        private void OpenAdminAuthenticationView(object parameter)
        {
            AdminAuthentication adminWindow = new AdminAuthentication();

            adminWindow.Show();

            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private void OpenClientView(object parameter)
        {
            MainClientPanel clientWindow = new MainClientPanel();

            clientWindow.Show();

            if (parameter is Window window)
            {
                window.Close();
            }
        }

        public static MainWindowViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = DIContainer.GetContainer().Resolve<MainWindowViewModel>();
                    return _instance;
                }
                else
                    return _instance;
            }
        }
    }
}
