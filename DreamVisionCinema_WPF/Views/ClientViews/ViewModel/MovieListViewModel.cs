﻿using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF_Logic.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DreamVisionCinema_WPF.Views.ClientViews.ViewModel
{
    public class MovieListViewModel : BaseViewModel
    {
        private MovieRepository repository;
        public ObservableCollection<Movie> Movies { get; set; }
        public ICommand MovieDetailsViewCommand { get; private set; }

        public MovieListViewModel()
        {
            repository = new MovieRepository();
            repository.ReadMoviesFromFile();
            Movies = new ObservableCollection<Movie>(repository.GetAllMovies());

            MovieDetailsViewCommand = new RelayCommand(OpenReservationModal);
        }

        private void OpenReservationModal(object parameter)
        {
            if (parameter is Movie movie)
            {
                Console.WriteLine(movie.Title);
                GlobalEventAggregator.RaiseViewChanged(new MovieDetailsViewModel(movie), "Detale Filmu");
            }
        }
    }
}