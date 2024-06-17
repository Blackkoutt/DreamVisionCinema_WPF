using DreamVisionCinema_WPF.Views.AdminViews;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using DreamVisionCinema_WPF_Logic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamVisionCinema_WPF.ViewModels.ClientViewModels
{
    public class ClientReservationListViewModel
    {
        public ObservableCollection<Reservation> reservationsList {  get; set; }
        private ReservationRepository repository;
        private MovieRepository movieRepository { get; set; }

        public ClientReservationListViewModel() 
        {
            movieRepository = new MovieRepository();
            movieRepository.ReadMoviesFromFile();
            repository = new ReservationRepository(movieRepository);
            repository.ReadReservationsFromFile();
            reservationsList = new ObservableCollection<Reservation>(repository.GetReservationsList());
            
        }
    }
}
