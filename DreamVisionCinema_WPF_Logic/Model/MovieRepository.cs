using DreamVisionCinema_WPF_Logic.Enums;
using DreamVisionCinema_WPF_Logic.Exceptions;
using DreamVisionCinema_WPF_Logic.Extensions;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using System.Data;
using System.Reflection;

namespace DreamVisionCinema_WPF_Logic.Model
{
    // Klasa ta implementuje interfejs poprzez który komunikuje się z nią kontroler
    public class MovieRepository : IMovieRepository
    {
        private List<Movie> movies; // Lista filmów

        public MovieRepository()
        {
            movies = new List<Movie>();
        }


        // Metoda odczytująca filmy z pliku
        public void ReadMoviesFromFile()
        {
            // Odczyt filmów z pliku tekstowego
            int b = movies.Count;
            int a = 1;
            string directoryPath = "../../../Database/Data";
            string fileName = "movies.txt";
            string filePath = Path.Combine(directoryPath, fileName);

            StreamReader sr;
            try
            {
                sr = new StreamReader(filePath);
            }
            catch
            {
                throw new CannotReadFileException("Błąd otwarcia pliku. Sprawdź czy plik movies.txt znajduje się w katalogu DreamVisionCinema_WPF_Logic.");
            }
            sr.ReadLine(); // Pomiń pierwszą linię - nagłówek pliku

            string id, title, date, price, duration, roomNumber, line;
            string[] strings_tab;

            // Czytaj linie do końca pliku
            while (!sr.EndOfStream) 
            {
                line = sr.ReadLine();
                strings_tab = line.Split(" "); // Rozdziel linię względem spacji

                if (strings_tab.Length != 7)
                {
                    throw new FileSyntaxException("Bład w składni pliku movies.txt. Sprawdź czy plik jest sformułowany według wzoru podanego w pierwszej linii.");
                }

                id = strings_tab[0];
                title = strings_tab[1].Replace("_", " ");
                date = strings_tab[2] + " " + strings_tab[3];
                price = strings_tab[4];
                duration = strings_tab[5];
                roomNumber = strings_tab[6];
                AddMovie(id, title, date, price, duration, roomNumber); // Dodaj film do listy
            }
            sr.Close();
        }


        // Metoda dodająca film do listy
        public void AddMovie(string? id, string title, string date, string price, string duration, string roomNumber)
        {
            DateTime Date;
            double Price;
            int RoomNumber, Id = 1; // Default value

            // Konwertowanie podanych ciągów znaków na odpowiednie typy
            if (id != null)
            {
                Id = Conversions.TryParseToInt(id, "Id musi być liczbą całkowitą.");
            }
            if(string.IsNullOrWhiteSpace(title))
            {
                throw new IncorrectParametrException("Należy podać tytuł filmu. Nie może być on pustym ciągiem znaków.");
            }
            Date = Conversions.TryParseToDateTime(date, "Data powinna byc zapisana w postaci: dd/MM/yyyy HH:mm.");
            RoomNumber = Conversions.TryParseToInt(roomNumber, "Numer pokoju musi być liczbą całkowitą.");
            price = price.Replace(".", ",");
            Price = Conversions.TryParseToDouble(price, "Cena powinna być wartością zmiennoprzecinkową np 23,99.");

            if (Date < DateTime.Now)
            {
                throw new IncorrectParametrException("Data nie może być wcześniejsza niż data bieżąca");
            }

            // Sprawdzenie poprawności parametrów
            if (Price < 0)
            {
                throw new IncorrectParametrException("Cena nie może być wartością ujemną.");
            }

            Validation.IsDurationCorrect(duration); // Duration musi być podane jako H:MM

            if (!Validation.IsValidRoomNumber(RoomNumber))
            {
                throw new NoRoomWithGivenNumberException("Brak sali o podanym numerze. Wybierz salę od 1 do 4");
            }

            // Sprawdzenie czy podana data i sala filmu nie koliduje z inną już obecną na liście
            CheckTimeCollisionsBetweenMovies(Date, duration, RoomNumber);

            // Jeśli metoda AddMovie została wywołana z poziomu administratora
            if (id == null && movies.Any()) 
            {
                // Id jest pobierane jako ostatnie id z listy + 1
                movies.Add(new Movie((getLastMovie().Id) + 1, title, Date, Price, duration, new Room(RoomNumber, (int)Rooms.NUMBER_OF_SEATS)));              
            }
            // Jeśli metoda AddMovie została wywołana podczas odczytu filmów z pliku
            else
            {
                // Id jest odczytaną wartością z pliku
                movies.Add(new Movie(Id, title, Date, Price, duration, new Room(RoomNumber, (int)Rooms.NUMBER_OF_SEATS)));
            }
        }


        // Metoda pobierająca ostatni film z listy
        private Movie getLastMovie()
        {
            return movies[movies.Count - 1];
        }


        // Metoda sprawdzająca kolizję z innymi filmami
        public void CheckTimeCollisionsBetweenMovies(DateTime date, string duration, int roomNumber)
        {
            DateTime newMovieStartDate = date;  // Godzina rozpoczecia dodawanego filmu
            TimeSpan timeSpan = TimeSpan.Parse(duration);   // Czas trwania dodawanego filmu
            DateTime newMovieEndDate = newMovieStartDate.Add(timeSpan); // Godzina zakończenia dodawanego filmu

            TimeSpan span;
            DateTime movieEndDate, movieStartDate;

            // Przejrzenie całej listy filmów
            foreach(Movie movie in movies)
            {
                // Jeśli dodawany film jest wyświetlany w tej samej sali co inny film
                if(movie.Room.Number == roomNumber)
                {
                    // Wyznaczenie daty rozpoczęcia i zakończenia danego filmu z listy
                    span = TimeSpan.Parse(movie.Duration);
                    movieStartDate = movie.Date;
                    movieEndDate = movie.Date.Add(span);

                    // Warunek sprawdzający wystąpienie kolizji
                    if ((movieStartDate >= newMovieStartDate && movieStartDate < newMovieEndDate) || (movieEndDate>newMovieStartDate && movieEndDate <= newMovieEndDate))
                    {
                        throw new MoviesCollisionException("Czas filmu koliduje innym filmem. Wybierz inną godzinę, datę lub salę.");
                    }
                }
               
            }
        }

        
        // Metoda usuwająca dany film o danym ID z listy (powiązana z rezerwacjami)
        public void RemoveMovie(int id)
        {
            movies.RemoveAll(x => x.Id == id);
        }


        // Metoda modyfikująca datę lub salę danego filmu (powiązana z rezerwacjami)
        public void ModifyMovieDateOrRoom(Movie movie, DateTime newDate, int newRoomNumber)
        {
            movie.Date = newDate;
            movie.Room.Number = newRoomNumber;
        }


        // Metoda modyfikująca cenę filmu
        public void ModifyMoviePrice(string id, string new_price)
        {
            // Konwertowanie ciągów znaków na właściwy typ
            int Id = Conversions.TryParseToInt(id, "Id powinno być liczbą całkowitą");
            double newPrice = Conversions.TryParseToDouble(new_price, "Cena powinna być wartością zmiennoprzecinkową np. 9.99");
            
            // Sprawdzenie poprawności podanych argumentów
            if (newPrice < 0)
            {
                throw new IncorrectParametrException("Cena nie może być wartością ujemną.");
            }
            Movie movie = GetOneMovie(Id);
            if (movie == null)
            {
                throw new NoMovieWithGivenIdException("Brak filmu o podanym Id.");
            }

            movie.Price = newPrice;
        }


        // Metoda wyświetlająca listę filmów
        public List<Movie> GetAllMovies() 
        {
            if (!movies.Any())
            {
                throw new MovieListIsEmptyException("Lista filmów jest pusta.");
            }
            return movies;
        }


        // Metoda pobierająca z listy film o danym id (ciąg znaków)
        public Movie GetOneMovie(string id)
        {
            // Konwersja ciągu znaków na właściwy typ
            int ID = Conversions.TryParseToInt(id, "ID powinno być liczbą całkowitą.");

            // Sprawdzenie poprawności podanego parametru
            Movie movie = movies.FirstOrDefault(m => m.Id == ID);
            if (movie == null)
            {
                throw new NoMovieWithGivenIdException("Brak filmu o podanym ID.");
            }
            return movie;
        }


        // Metoda pobierająca z listy film o danym id (liczba całkowita)
        public Movie GetOneMovie(int id)
        {

            // Sprawdzanie poprawności podawanego parametru odbywa się w innych metodach
            return movies.FirstOrDefault(m => m.Id == id);
        }

        
        // Metoda wyszukująca daną frazę w liście filmów
        public List<Movie> FilterList(string userInput)
        {
            PropertyInfo[] properties = typeof(Movie).GetProperties();  // Atrybuty klasy Movie

            // Utwórz przefiltrowaną listę filmów
            List<Movie> filteredMovies = movies.Where(movie =>
            {
                return properties.Any(property =>
                {
                    string? propertyValue = property.GetValue(movie).ToString(); // Pobranie wartości kolejnych atrybutów kolejnych filmów

                    // Dopasowanie wartości pobranej od użytkownika do wartości atrybutu
                    // Szukanie ciągu znaków podanego przez użytkownika znajdującego się w wartości atrybutu
                    return propertyValue != null && propertyValue.IndexOf(userInput, StringComparison.OrdinalIgnoreCase) >= 0;
                });
            }).ToList();

            // Jeśli lista jest pusta wyrzuć wyjątek
            if (!filteredMovies.Any())
            {
                throw new CannotFindMatchingMovieException("Brak wyników wyszukiwania.");
            }
            return filteredMovies;
        }


        // Metoda sortująca rosnąco listę filmów na podstawie podanego atrybutu
        public List<Movie> SortAscending(string attribute)
        {
            // Ignorowana jest wielkość liter i polskie znaki
            attribute = attribute.ToLower();
            List<Movie> SortedMovies = new List<Movie>();

            // Sortuj listę według podanego atrybutu
            switch (attribute)
            {
                case "id":
                    {
                        SortedMovies = movies.OrderBy(movie => movie.Id).ToList();
                        break;
                    }
                case "tytul":
                case "tytuł":
                    {
                        SortedMovies = movies.OrderBy(movie => movie.Title).ToList();
                        break;
                    }
                case "data":
                    {
                        SortedMovies = movies.OrderBy(movie => movie.Date).ToList();
                        break;
                    }
                case "cena":
                    {
                        SortedMovies = movies.OrderBy(movie => movie.Price).ToList();
                        break;
                    }
                case "czas":
                    {
                        SortedMovies = movies.OrderBy(movie => movie.Duration).ToList();
                        break;
                    }
                case "długość":
                    {
                        SortedMovies = movies.OrderBy(movie => movie.Duration).ToList();
                        break;
                    }
                case "sala":
                    {
                        SortedMovies = movies.OrderBy(movie => movie.Room.Number).ToList();
                        break;
                    }
                // Jeśli podano zły atrybut wyrzuć wyjątek
                default:
                    {
                        throw new BadAttributeException($"Brak atrybutu o podanej nazwie: {attribute}.");
                    }
            }

            // Jeśli lista jest pusta wyrzuć wyjątek
            if (!SortedMovies.Any())
            {
                throw new ListIsEmptyException("Lista jest pusta");
            }
            return SortedMovies;
        }


        // Metoda sortująca malejąco listę filmów na podstawie podanego atrybutu
        public List<Movie> SortDescending(string attribute)
        {
            // Ignorowana jest wielkość liter i polskie znaki
            attribute = attribute.ToLower();
            List<Movie> SortedMovies = new List<Movie>();

            // Sortuj listę według podanego atrybutu
            switch (attribute)
            {
                case "id":
                    {
                        SortedMovies = movies.OrderByDescending(movie => movie.Id).ToList();
                        break;
                    }
                case "tytul":
                case "tytuł":
                    {
                        SortedMovies = movies.OrderByDescending(movie => movie.Title).ToList();
                        break;
                    }
                case "data":
                    {
                        SortedMovies = movies.OrderByDescending(movie => movie.Date).ToList();
                        break;
                    }
                case "cena":
                    {
                        SortedMovies = movies.OrderByDescending(movie => movie.Price).ToList();
                        break;
                    }
                case "czas":
                    {
                        SortedMovies = movies.OrderByDescending(movie => movie.Duration).ToList();
                        break;
                    }
                case "długość":
                    {
                        SortedMovies = movies.OrderByDescending(movie => movie.Duration).ToList();
                        break;
                    }
                case "sala":
                    {
                        SortedMovies = movies.OrderByDescending(movie => movie.Room.Number).ToList();
                        break;
                    }
                // Jeśli podano zły atrybut wyrzuć wyjątek
                default:
                    {
                        throw new BadAttributeException($"Brak atrybutu o podanej nazwie: {attribute}.");
                    }
            }

            // Jeśli lista jest pusta wyrzuć wyjątek
            if (!SortedMovies.Any())
            {
                throw new ListIsEmptyException("Lista jest pusta");
            }
            return SortedMovies;
        } 


        // Metoda zapisująca filmy do pliku
        public void SaveMoviesToFile()
        {
            string directoryPath = "../../../Database/Data";
            string fileName = "movies.txt";
            string filePath = Path.Combine(directoryPath, fileName);

            StreamWriter sw = new StreamWriter(filePath);
            string title, date_hour;

            sw.WriteLine("//id tytuł data godzina cena czas_trwania numer_sali"); // Nagłówek pliku
            foreach (Movie movie in movies) 
            {
                title = movie.Title.Replace(" ", "_");
                date_hour = movie.Date.ToString("dd/MM/yyyy HH:mm");
                sw.WriteLine($"{movie.Id} {title} {date_hour} {movie.Price} {movie.Duration} {movie.Room.Number}");
            }
            sw.Close();
        }

    }
}