using System;
using System.Windows;
using System.Windows.Input;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.Views.ClientViews;
using DreamVisionCinema_WPF_Logic.Model;

namespace DreamVisionCinema_WPF.ViewModels.ClientViewModels
{
    public class MovieDetailsViewModel
    {
        public Movie Movie { get; private set; }
        public MovieDetailsViewModel()
        { }

        public MovieDetailsViewModel(Movie movie)
        {
            Movie = movie;
            OpenSeatChoiceCommand = new RelayCommand(OpenSeatChoice);
        }

        public ICommand OpenSeatChoiceCommand { get; private set; }

        private void OpenSeatChoice(object parameter)
        {
            SeatReservationWindow dialog = new SeatReservationWindow();
            dialog.Show();
        }
    }
}