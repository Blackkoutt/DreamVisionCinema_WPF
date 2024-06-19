using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Enums;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.ViewModels.ClientViewModels;
using DreamVisionCinema_WPF.Views.AdminViews;
using DreamVisionCinema_WPF_Logic.Exceptions;
using DreamVisionCinema_WPF_Logic.Interfaces;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using DreamVisionCinema_WPF_Logic.Model;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.AdminViewModels
{
    public class AdminAuthenticationViewModel : BaseViewModel
    {
        private static AdminAuthenticationViewModel _instance = null;
        public ICommand LogInCommand { get; set; }
        public ICommand DragMoveCommand => base.DragCommand;
        private ILogin _login;
        private IMovieRepository movieRepository;
        private IReservationRepository reservationRepository;

        private string? username;
        private string? password;

        public AdminAuthenticationViewModel(ILogin login, IMovieRepository movieRepository, IReservationRepository reservationRepository)
        {
            this.movieRepository = movieRepository;
            this.reservationRepository = reservationRepository;
            _login = login; 
            LogInCommand = new RelayCommand(LogInAsAdmin);
        }

        private void LogInAsAdmin(object parameter)
        {
            bool isValidLoginAndPassword = false;
            if(Username == null || Password == null)
            {
                MakeAlert("Należy podać login oraz hasło.", AlertTypeEnum.Error, true);
                return;
            }
            try
            {
                isValidLoginAndPassword = _login.SignIn(Username, Password);
            }
            catch (CannotReadFileException CRFE)
            {
                MakeAlert(CRFE.Message, AlertTypeEnum.Error, true);
                return;
            }
            catch (Exception ex)
            {
                MakeAlert("Wystąpił nieznany błąd: " + ex.Message, AlertTypeEnum.Error, true);
                return;
            }

            if (isValidLoginAndPassword)
            {
                MainAdminPanel adminPanel = new MainAdminPanel();

                adminPanel.Show();

                if (parameter is Window window)
                {
                    window.Close();
                }

                MakeAlert("Pomyślnie zalogowano jako administrator", AlertTypeEnum.Success, true);
            }
            else
            {
                MakeAlert("Błędny login lub hasło", AlertTypeEnum.Error, true);
            }
        }
        protected override void CloseWindow(object parameter)
        {
            movieRepository.SaveMoviesToFile();
            reservationRepository.SaveReservationsToFile();

            if (parameter is Window window)
            {
                window.Close();
            }
        }

        public static AdminAuthenticationViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = DIContainer.GetContainer().Resolve<AdminAuthenticationViewModel>();
                    return _instance;
                }
                else
                    return _instance;
            }
        }
        public string? Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        public string? Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }
    }
}
