using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Enums;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.ViewModels.AdminViewModels;
using DreamVisionCinema_WPF.Views.AdminViews;
using DreamVisionCinema_WPF_Logic.Exceptions;
using DreamVisionCinema_WPF_Logic.Interfaces;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using DreamVisionCinema_WPF_Logic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.ClientViewModels
{
    public class ClientReservationListViewModel : BaseViewModel
    {
        private static ClientReservationListViewModel _instance = null;

        public ObservableCollection<Reservation> reservationsList {  get; set; }
        private IReservationRepository repository;
        public ICommand SearchReservationListCommand { get; set; }
        private string? searchValue;


        public ClientReservationListViewModel(IReservationRepository reservationRepository) 
        {
            repository = reservationRepository;
            reservationsList = new ObservableCollection<Reservation>(repository.GetReservationsList());
            SearchReservationListCommand = new RelayCommand(SearchReservationList);
            LoadOrRefreshReservatonList();        }

        public void LoadOrRefreshReservatonList()
        {
            try
            {
                ReservationsList = new ObservableCollection<Reservation>(repository.GetReservationsList());

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
        private void SearchReservationList(object parameter)
        {
            string search_value = (SearchValue != null) ? SearchValue : string.Empty;

            if (ReservationsList.Any())
            {
                try
                {
                    ReservationsList = new ObservableCollection<Reservation>(repository.FilterList(search_value));
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


        public static ClientReservationListViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = DIContainer.GetContainer().Resolve<ClientReservationListViewModel>();
                    return _instance;
                }
                else
                    return _instance;
            }
        }
        public ObservableCollection<Reservation> ReservationsList
        {
            get { return reservationsList; }
            set
            {
                reservationsList = value;
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
