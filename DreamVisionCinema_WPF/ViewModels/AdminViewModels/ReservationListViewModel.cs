using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Enums;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.Views.AdminViews;
using DreamVisionCinema_WPF_Logic.Exceptions;
using DreamVisionCinema_WPF_Logic.Interfaces;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using DreamVisionCinema_WPF_Logic.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.AdminViewModels
{
    public class ReservationListViewModel : BaseViewModel
    {
        private static ReservationListViewModel _instance = null;

        private ObservableCollection<Reservation> reservationList;
        private IReservationRepository reservationRepository;
        private string? searchValue;

        public ICommand SearchReservationListCommand { get; set; }

        public ReservationListViewModel(IReservationRepository reservationRepository)
        {
            reservationList = new ObservableCollection<Reservation>();  
            this.reservationRepository = reservationRepository;
            SearchReservationListCommand = new RelayCommand(SearchReservationList);
            LoadOrRefreshReservatonList();
        }

        private void SearchReservationList(object parameter)
        {
            string search_value = (SearchValue != null) ? SearchValue : string.Empty;

            if (ReservationList.Any())
            {
                try
                {
                    ReservationList = new ObservableCollection<Reservation>(reservationRepository.FilterList(search_value));
                }
                catch (CannotFindMatchingReservationException CFMRE)
                {
                    MakeAlert(CFMRE.Message, AlertTypeEnum.Info, true);
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

        public void LoadOrRefreshReservatonList()
        {
            try
            {
                reservationList = new ObservableCollection<Reservation>(reservationRepository.GetReservationsList());

            }
            catch (ReservationListIsEmptyException RLIEE)
            {
                MakeAlert(RLIEE.Message, AlertTypeEnum.Error, true);
                return;
            }
            catch (Exception ex)
            {
                MakeAlert(ex.Message, AlertTypeEnum.Error, true);
                return;
            }
        }

        public static ReservationListViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = DIContainer.GetContainer().Resolve<ReservationListViewModel>();
                    return _instance;
                }
                else
                    return _instance;
            }
        }
        public ObservableCollection<Reservation> ReservationList
        {
            get { return reservationList; }
            set
            {
                reservationList = value;
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
