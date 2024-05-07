﻿using DreamVisionCinema_WPF_Logic.Model;
using System.Windows;
using System.Windows.Controls;

namespace DreamVisionCinema_WPF.Views.ClientViews
{
    /// <summary>
    /// Interaction logic for MovieListView.xaml
    /// </summary>
    public partial class MovieListView: UserControl
    {
        MovieRepository repository = new MovieRepository();
        public MovieListView()
        {
            InitializeComponent();

            repository.ReadMoviesFromFile();

            List<Movie> movies = new List<Movie>();
            movies.Add(new Movie(1, "Najnowszy film bardzo dobry", DateTime.Now, 20.99, "1:30h", new Room(1, 100)));
            movies.Add(new Movie(2, "Film 2", DateTime.Now, 20.99, "1:30h", new Room(1, 100)));

            movies = repository.GetAllMovies();

            moviesList.ItemsSource = movies;
        }
    }
}
