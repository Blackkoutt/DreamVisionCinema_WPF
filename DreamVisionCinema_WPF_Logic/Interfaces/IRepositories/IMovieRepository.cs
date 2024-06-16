using DreamVisionCinema_WPF_Logic.Model;

namespace DreamVisionCinema_WPF_Logic.Interfaces.IRepositories
{
    public interface IMovieRepository
    {
        void ReadMoviesFromFile();
        void AddMovie(string? id, string title, string date, string price, string duration, string roomNumber, string description, string ageCategory, string pathToPoster);
        void CheckTimeCollisionsBetweenMovies(DateTime date, string duration, int roomNumber);
        void RemoveMovie(int id);
        void ModifyMovieDateOrRoom(Movie movie, DateTime newDate, int newRoomNumber);
        void ModifyMoviePrice(string id, string new_price);
        List<Movie> GetAllMovies();
        Movie GetOneMovie(string id);
        Movie GetOneMovie(int id);
        List<Movie> FilterList(string userInput);
        List<Movie> SortAscending(string attribute);
        List<Movie> SortDescending(string attribute);
        void SaveMoviesToFile();
    }
}