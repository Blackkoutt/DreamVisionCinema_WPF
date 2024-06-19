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
        void ModifyMoviePriceDescriptionAgeCategoryPathToPoster(string id, string new_price, string new_description, string new_age_category, string new_path_to_poster);
        List<Movie> GetAllMovies();
        Movie GetOneMovie(string id);
        Movie GetOneMovie(int id);
        List<Movie> FilterList(string userInput);
        List<Movie> SortAscending(string attribute);
        List<Movie> SortDescending(string attribute);
        void SaveMoviesToFile();
    }
}