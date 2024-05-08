using DreamVisionCinema_WPF_Logic.Exceptions;
using DreamVisionCinema_WPF_Logic.Extensions;
using DreamVisionCinema_WPF_Logic.Interfaces;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;

namespace DreamVisionCinema_WPF_Logic.Model
{
    // Klasa ta implementuje interfejs poprzez który komunikuje się z nią kontroler
    public class ReservationRepository : IReservationRepository
    {
        private List<Reservation> reservations;
        private IMovieRepository movies;

        public ReservationRepository(IMovieRepository movies)
        {
            reservations = new List<Reservation>();
            this.movies = movies;
        }


        // Metoda tworząca nową rezerwację
        public void MakeReservation(string? reservation_id, string movie_id, List<string> seatsNumbers)
        {
            int Movie_Id, Reservation_Id = 1; // Default value
            List<int> SeatsNumbers = new List<int>();

            // Konwersja ciągów znaków na poprawny typ
            if(reservation_id != null)
            {
                Reservation_Id = Conversions.TryParseToInt(reservation_id, "ID rezerwacji powinno być liczbą całkowitą");
            }
            
            Movie_Id = Conversions.TryParseToInt(movie_id, "ID filmu powinno być liczbą całkowitą");

            foreach(string str in seatsNumbers)
            {
                SeatsNumbers.Add(Conversions.TryParseToInt(str, "Miejsce powinno być liczbą całkowitą"));
            }

            // Sprawdzenie poprawności podanych parametrów
            Movie movie = movies.GetOneMovie(Movie_Id);
            if(movie == null)
            {
                throw new NoMovieWithGivenIdException("Brak filmu o podanym Id.");
            }

            // Sprawdzenie czy wybrane miejsca istnieją i są dostępne
            movie.Room.CheckSeatsAvailable(SeatsNumbers);

            // Jeśli są dostępne zaznaczamy je jako zajęte i pobieramy
            List<Seat> seats = movie.Room.MarkSeatsAsReservatedAndGet(SeatsNumbers);

            // Jeśli dodawanie rezerwacji zostało wywołane przez administratora
            if (reservation_id == null && reservations.Any())
            {
                // Id dokonanej rezerwacji jest o 1 większe od id ostatniej rezerwacji na liście
                reservations.Add(new Reservation((GetLastReservation().Id) + 1, movie, seats));
            }
            // Jeśli dodawanie rezerwacji zostało wywołane przy odczycie rezerwacji z pliku
            else
            {
                // Id rezerwacji jest wartością odczytaną z pliku tekstowego
                reservations.Add(new Reservation(Reservation_Id, movie, seats));
            }


            string Seats = string.Join(",", seats); // Łączenie listy miejsc w ciąg znaków (miejsca oddzielone przecinkiem)

            // Dla ostatniej rezerwacji na liście (nowo dodanej) tworzony jest bilet
            reservations[reservations.Count - 1].Ticket.SaveTicketToFile(movie, Seats); 
        }


        // Metoda tworząca i zwracająca bilet celem wyświetlenia na ekranie
        public string[] GetTicket()
        {
            Reservation res = GetLastReservation(); // Pobranie ostatniej rezerwacji (nowo dodanej)
            return res.Ticket.RenderTemplate(res);
        }


        // Metoda sprawdzająca czy którykolwiek z filmów na który istnieje rezerwacja został zmodyfikowany
        public List<string> CheckModificatedMoviesWithReservation()
        {
            List<string> info = new List<string>();


            // Lista zmodyfikowanych filmów zapisywana jest w pliku tymczasowym
            string directoryPath = "../../../Database/Temp"; 
            string fileName = "modificated_movies.txt";
            string filePath = Path.Combine(directoryPath, fileName);

            StreamReader sr;
            try
            {
                sr = new StreamReader(filePath);
            }
            catch (Exception ex)
            {
                throw new CannotReadFileException("Sprawdź czy plik modificated_movies.txt znajduje się w katalogu: Temp.");
            }

            while (!sr.EndOfStream)
            {
                info.Add(sr.ReadLine());
            }
            sr.Close();


            // Wyczyść zawartość pliku
            StreamWriter sw = new StreamWriter(filePath);
            sw.Close();

            return info;    // Zwróć odczytane dane
        }
        
        // Tworzy brakujący plik tymczasowy zmodyfikowanych filmów
        public void CreateTempModificationFile()
        {
            string directoryPath = "../../../Database/Temp";
            string fileName = "modificated_movies.txt";
            string filePath = Path.Combine(directoryPath, fileName);
            StreamWriter sw = new StreamWriter(filePath);
            sw.Close();
        }

        // Tworzy brakujący plik tymczasowy usuniętych filmów
        public void CreateTempDeleteFile()
        {
            string directoryPath = "../../../Database/Temp";
            string fileName = "deleted_movies.txt";
            string filePath = Path.Combine(directoryPath, fileName);
            StreamWriter sw = new StreamWriter(filePath);
            sw.Close();
        }


        // Metoda sprawdzająca czy którykolwiek z filmów na który istniała rezerwacja został usunięty (usunięta została też rezerwacja)
        public List<string> CheckDeletedReservations()
        {
            List<string> info = new List<string>();

            // Lista usuniętych rezerwacji zapisywana jest w pliku tymczasowym
            string directoryPath = "../../../Database/Temp";
            string fileName = "deleted_movies.txt";
            string filePath = Path.Combine(directoryPath, fileName);

            StreamReader sr;
            try
            {
                sr = new StreamReader(filePath);
            }
            catch (Exception ex)
            {
                throw new CannotReadFileException("Sprawdź czy plik deleted_movies.txt znajduje się w katalogu: Temp");
            }
            while (!sr.EndOfStream) 
            {
                info.Add(sr.ReadLine());
            }
            sr.Close();

            // Wyczyść zawartość pliku
            StreamWriter sw = new StreamWriter(filePath);
            sw.Close();

            return info;    // Zwróć odczytane dane
        }


        // Metoda pobierająca rezerwację o danym id
        private Reservation GetOneReservation(int id)
        {
            return reservations.FirstOrDefault(r => r.Id == id);
        }


        // Metoda usuwająca z listy rezerwację o danym id
        public void DeleteReservation(string id)
        {
            // Konwersja podanego ciągu znaków na odpowiedni typ
            int Id = Conversions.TryParseToInt(id, "ID musi być liczbą całkowitą");

            // Sprawdzenie poprawności podanego parametru
            Reservation res = GetOneReservation(Id);
            if (res == null)
            {
                throw new NoReservationWithGivenIdException("Brak rezerwacji o podanym Id.");
            }

            res.Ticket.DestroyTicket(res.Movie.Title);  // Usunięcie pliku z biletem dotyczącego danej rezerwacji

            res.Movie.Room.MarkSeatsAsAvailable(res.Seats); //Ustawienie miejsc podlegających danej rezerwacji jako wolne

            reservations.RemoveAll(reservation => reservation == res);  // Usunięcie rezerwacji z listy
        }


        // Metoda sprawdzająca czy dany film posiada jakąkolwiek rezerwację
        public bool IsMovieHaveReservation(Movie movie)
        {
            return reservations.Any(reservation => reservation.Movie == movie);
        }

        public void DestroyTicketJPG(string resID)
        {
            int id = Conversions.TryParseToInt(resID, "ID musi być liczbą całkowitą");
            Reservation res = GetOneReservation(id);
            if(res == null)
            {
                throw new NoReservationWithGivenIdException("Brak rezerwacji o podanym ID");
            }
            //title = title.Replace(" ", "_");
            string directoryPath = "../../../Database/TicketsJPG";
            string fileName = res.Ticket.Id + "_" + res.Movie.Title + ".jpg";
            string filePath = Path.Combine(directoryPath, fileName);

            try
            {
                File.Delete(filePath);  // Usuń plik
            }
            catch
            {
                throw new CannotDestroyTicketException($"Nie udało się usunąć pliku z biletem o nazwie {fileName}");
            }
        }

        // Metoda modyfikująca datę lub salę filmu który posiada jakąkolwiek rezerwację
        public void ModifyMovieDateOrRoomWithReservation(string id, string new_date, string new_room_number)
        {
            // Konwersja podanego ciągu znaków na odpowiedni typ
            int Id = Conversions.TryParseToInt(id, "Id powinno być wartością całkowitą");
            DateTime NewDate;
            int newRoomNumber;

            // Sprawdzenie poprawności podanego ciągu znaków
            Movie movie = movies.GetOneMovie(Id);
            if (movie == null)
            {
                throw new NoMovieWithGivenIdException("Brak filmu o podanym Id.");
            }

            int previousRoomNumber = movie.Room.Number; // Aktualna sala filmu
            string previousDate = movie.Date.ToString("dd/MM/yyyy HH:mm");  // Aktualna data filmu

            // Jeśli zmieniono datę 
            if (new_date != "")
            {
                NewDate = Conversions.TryParseToDateTime(new_date, "Sprawdź czy data jest podana w formacie dd/MM/yyyy HH:mm");
                if (NewDate < DateTime.Now)
                {
                    throw new IncorrectParametrException("Data nie może być wcześniejsza niż data bieżąca!");
                }
                // Jeśli zmieniono też salę
                if (new_room_number != "")
                {
                    newRoomNumber = Conversions.TryParseToInt(new_room_number, "Numer sali powinien być wartością całkowitą");
                    if (!Validation.IsValidRoomNumber(newRoomNumber))
                    {
                        throw new NoRoomWithGivenNumberException("Brak sali o podanym numerze");
                    }
                    // Sprawdzenie kolizji z innymi filmami
                    movies.CheckTimeCollisionsBetweenMovies(NewDate, movie.Duration, newRoomNumber);
                    movies.ModifyMovieDateOrRoom(movie, NewDate, newRoomNumber);    // Modyfikacja daty i sali filmu
                }
                // Jeśli zmieniono tylko datę
                else 
                {
                    // Sprawdzenie kolizji z innymi filmami
                    movies.CheckTimeCollisionsBetweenMovies(NewDate, movie.Duration, movie.Room.Number);
                    movies.ModifyMovieDateOrRoom(movie, NewDate, movie.Room.Number);    // Modyfikacja daty filmu
                }
                
            }
            // Jeśli zmieniono tylko salę
            else if (new_room_number != "")
            {
                newRoomNumber = Conversions.TryParseToInt(new_room_number, "Numer sali powinien być wartością całkowitą");
                if (!Validation.IsValidRoomNumber(newRoomNumber))
                {
                    throw new NoRoomWithGivenNumberException("Brak sali o podanym numerze");
                }
                // Sprawdzenie kolizji z innymi filmami
                movies.CheckTimeCollisionsBetweenMovies(movie.Date, movie.Duration, newRoomNumber);
                movies.ModifyMovieDateOrRoom(movie, movie.Date, newRoomNumber); // Modyfikacja sali filmu
            }

            // Jeśli zmodyfikowany film posiada jakąkolwiek rezerwację
            if (IsMovieHaveReservation(movie))
            {
                // Pobranie wszystkich rezerwacji dotyczących danego filmu
                List<Reservation> reservation_list = GetReservationForMovie(movie);

                // Zapis informacji o zmodyfikowanych dacie lub sali filmu do pliku tymczasowego
                string directoryPath = "../../../Database/Temp";
                string fileName = "modificated_movies.txt";
                string filePath = Path.Combine(directoryPath, fileName);

                StreamWriter sw = new StreamWriter(filePath, true);
                
                foreach (Reservation res in reservation_list)
                {
                    string seats = string.Join(", ", res.Seats);                 
                    res.Ticket.DestroyTicket(res.Movie.Title);  // Usunięcie poprzedniego pliku z biletem
                    res.Ticket.SaveTicketToFile(res.Movie, seats);  // Stworzenie zaktualizowanego pliku z biletem
                }
                // Jeśli zmieniono date
                if (previousDate != movie.Date.ToString("dd/MM/yyyy HH:mm"))
                {
                    sw.WriteLine($"[!] Zmieniono datę seansu filmu: \"{movie.Title}\".");
                    sw.WriteLine($"[!] Poprzednia data: {previousDate}. Nowa data: {movie.Date.ToString("dd/MM/yyyy HH:mm")}.");
                }
                // Jeśli zmieniono numer sali
                if (previousRoomNumber != movie.Room.Number)
                {
                    sw.WriteLine($"[!] Zmieniono salę dla filmu: \"{movie.Title}\".");
                    sw.WriteLine($"[!] Poprzednia sala: {previousRoomNumber}. Nowa sala: {movie.Room.Number}.");
                }
                sw.WriteLine("[V] Wszystkie bilety dotyczące tego filmu zostały zaktualizowane!");
                sw.Close();
            }
        }

        public void RemoveTicketForReservatedMovies(string id)
        {
            int Id = Conversions.TryParseToInt(id, "ID musi być liczbą całkowitą");

            // Sprawdzenie poprawności podanego parametru
            Movie movie = movies.GetOneMovie(Id);
            if (movie == null)
            {
                throw new NoMovieWithGivenIdException("Brak filmu o podanym Id.");
            }
            if (IsMovieHaveReservation(movie))
            {
                List<Reservation> reservation_list = GetReservationForMovie(movie);
                foreach(Reservation res in reservation_list)
                {
                    DestroyTicketJPG(res.Id.ToString());
                }
            }
        }

        // Metoda usuwająca film który posiada rezerwację
        public void RemoveMovieWithReservation(string id)
        {
            // Konwersja podanego ciągu znaków na poprawny typ
            int Id = Conversions.TryParseToInt(id, "ID musi być liczbą całkowitą");

            // Sprawdzenie poprawności podanego parametru
            Movie movie = movies.GetOneMovie(Id);
            if (movie == null)
            {
                throw new NoMovieWithGivenIdException("Brak filmu o podanym Id.");
            }

            // Jeśli film posiada jakąkolwiek rezerwacje
            if (IsMovieHaveReservation(movie))
            {
                // Pobierane są wszystkie rezerwacje na dany film
                List<Reservation> reservation_list = GetReservationForMovie(movie);


                // Zapisanie informacji o usuniętych rezerwacjach do pliku (usuwając film usuwamy też rezerwację)
                string directoryPath = "../../../Database/Temp";
                string fileName = "deleted_movies.txt";
                string filePath = Path.Combine(directoryPath, fileName);

                StreamWriter sw = new StreamWriter(filePath, true);

                foreach(Reservation res in reservation_list)
                {
                    sw.WriteLine($"[!] Odwołano rezerwacje na film: \"{movie.Title}\" w dniu: {movie.Date.ToString("dd/MM/yyyy HH:mm")}.");
                    sw.WriteLine($"[V] Środki o wartości: {res.Ticket.CalculatePrice(movie.Price, res.Seats.Count)}$ zostały zwrócone na konto.");
                    res.Ticket.DestroyTicket(res.Movie.Title);  // Usunięcie pliku z biletem
                }
                sw.Close();
      
                DeleteGivenReservations(reservation_list); // Usunięcie wszytskich rezerwacji dotyczących danego filmu
            }         
            movies.RemoveMovie(movie.Id); // Usunięcie filmu
        }


        // Metoda zwracająca listę rezerwacji na dany film
        public List<Reservation> GetReservationForMovie(Movie movie)
        {
            return reservations.Where(reservation => reservation.Movie.Id == movie.Id).ToList();
        }


        // Metoda usuwająca podane rezerwacje z listy
        private void DeleteGivenReservations(List<Reservation> res)
        {
            reservations.RemoveAll(reservation => res.Contains(reservation));
        }


        // Metoda zwracająca najbardziej popularne filmy (kryterium - najwięcej zajętych miejsc)
        public Dictionary<string, int> GetMostPopularMovies()
        {
            Dictionary<string, int> MostPopularMovies = new Dictionary<string, int>();

            foreach (Reservation res in reservations)
            {
                if (MostPopularMovies.ContainsKey(res.Movie.Title))
                {
                    MostPopularMovies[res.Movie.Title] += res.Seats.Count;
                }
                else
                {
                    MostPopularMovies.Add(res.Movie.Title, res.Seats.Count);
                }
            }
            // Jeśli lista filmów jest pusta wyrzuć wyjątek
            if (!MostPopularMovies.Any())
            {
                throw new ListIsEmptyException("Lista jest pusta");
            }
            // Sortuj malejąco względem ilości zajętych miejsc
            return MostPopularMovies.OrderByDescending(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }


        // Metoda zwracająca najbardziej dochodowe filmy (kryterium - całkowita cena sprzedanych biletów na film)
        public Dictionary<string, double> GetMoviesIncome()
        {
            Dictionary<string, double> MoviesStats = new Dictionary<string, double>();

            foreach(Reservation res in reservations)
            {
                if (MoviesStats.ContainsKey(res.Movie.Title))
                {
                    MoviesStats[res.Movie.Title] += Math.Round(res.Movie.Price * res.Seats.Count, 2); 
                }
                else
                {
                    MoviesStats.Add(res.Movie.Title, Math.Round(res.Movie.Price * res.Seats.Count, 2));
                }
            }
            // Jeśli lista filmów jest pusta wyrzuć wyjątek
            if (!MoviesStats.Any())
            {
                throw new ListIsEmptyException("Lista jest pusta");
            }
            // Sortuj malejąco względem dochodów z filmu
            return MoviesStats.OrderByDescending(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }


        // Metoda zwracająca całkowity dochód kina
        public double GetTotalIncome()
        {
            double total = 0;  
            foreach (Reservation res in reservations)
            {
                total += res.Movie.Price * res.Seats.Count;
            }
            return total;
        }


        // Metoda zwracająca ostatnią rezerwację z listy
        public Reservation GetLastReservation()
        {
            // Jeśli lista jest pusta wyrzuć wyjątek
            if (!reservations.Any())
            {
                throw new ReservationListIsEmptyException("Lista rezerwacji jest pusta.");
            }
            return reservations[reservations.Count - 1];
        }


        // Metoda zwracająca listę rezerwacji
        public List<Reservation> GetReservationsList()
        {
            if(!reservations.Any())
            {
                throw new ReservationListIsEmptyException("Lista rezerwacji jest pusta.");
            }
            return reservations;
        }


        // Metoda odczytująca rezerwacje z pliku
        public void ReadReservationsFromFile()
        {
            string directoryPath = "../../../Database/Data";
            string fileName = "reservations.txt";
            string filePath = Path.Combine(directoryPath, fileName);

            StreamReader sr;         
            try
            {
                sr = new StreamReader(filePath);
            }
            catch
            {
                throw new CannotReadFileException("Błąd otwarcia pliku. Sprawdź czy plik reservations.txt znajduje się w katalogu DreamVisionCinema_WPF_Logic.");
            }
            sr.ReadLine(); // Pierwsza linia pliku to nagłówek

            string line, reservation_id, movie_id;
            string[] strings_tab;

            List<string> seats_numbers = new List<string>();

            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                strings_tab = line.Split(" ");  // Podziel linię względem spacji

                if (strings_tab.Length != 3)
                {
                    throw new FileSyntaxException("Bład w składni pliku reservations.txt. Sprawdź czy plik jest sformułowany według wzoru podanego w pierwszej linii.");
                }

                reservation_id = strings_tab[0];
                movie_id = strings_tab[1];
                strings_tab = strings_tab[2].Split(",");    // Podziel względem przecinka
                seats_numbers = strings_tab.ToList();

                MakeReservation(reservation_id, movie_id, seats_numbers);   // Utwórz rezerwację
            }
            sr.Close();
        }



        // Metoda zapisująca rezerwacje do pliku
        public void SaveReservationsToFile()
        {
            string directoryPath = "../../../Database/Data";
            string fileName = "reservations.txt";
            string filePath = Path.Combine(directoryPath, fileName);

            StreamWriter sw = new StreamWriter(filePath);
            sw.WriteLine("// id_rezerwacji id_filmu numery_miejsc");
            foreach(Reservation res in reservations)
            {
                string seats = string.Join(",", res.Seats); // Złącz listę miejsc przecinkami
                sw.WriteLine($"{res.Id} {res.Movie.Id} {seats}");
            }
            sw.Close();
        }
        
    }
}