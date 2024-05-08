using DreamVisionCinema_WPF_Logic.Exceptions;

namespace DreamVisionCinema_WPF_Logic.Model
{
    public class Ticket
    {
        private int id; // Id biletu
        private int price;  // Cena biletu
        // Szablon biletu
        private string[] ticketTemplate =
        {
            @"              ╔════════════════════════════════════════════════════════════════════════════════════════════════╗
              ║------------------------------------------------------------------------------------------------║
              ║[][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][]║
              ║------------------------------------------------------------------------------------------------║
              ║ ____                        __     ___     _                ____ _                             ║
              ║|  _ \ _ __ ___  __ _ _ __ __\ \   / (_)___(_) ___  _ __    / ___(_)_ __   ___ _ __ ___   __ _  ║
              ║| | | | '__/ _ \/ _` | '_ ` _ \ \ / /| / __| |/ _ \| '_ \  | |   | | '_ \ / _ \ '_ ` _ \ / _` | ║
              ║| |_| | | |  __/ (_| | | | | | \ V / | \__ \ | (_) | | | | | |___| | | | |  __/ | | | | | (_| | ║
              ║|____/|_|  \___|\__,_|_| |_| |_|\_/  |_|___/_|\___/|_| |_|  \____|_|_| |_|\___|_| |_| |_|\__,_| ║
              ║------------------------------------------------------------------------------------------------║
              ║[][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][]║
              ║------------------------------------------------------------------------------------------------║
              ║            ticket id:             │                                                            ║
              ║                {0,-19}│                              ▓▒▓                           ║
              ║    │║█║││║█│█║║█│║█││█║│█║││█║    │                           ▒▓▒                              ║
              ║    │║█║││║█│█║║█│║█││█║│█║││█║    │                        ▓▒▓                                 ║
              ║                                   │                     ▓▒                                     ║
              ║      date:           duration:    │                    ▓░▓░▓░▓░▓░▓░▓░▓░                        ║
              ║ {1,-1}       {2,-1}      │                    ████████████████                        ║
              ║                                   │                    ████████████████                        ║
              ║  room: {3,-27}│                    ████████████████                        ║
              ║                                   │                    ████████████████                        ║
              ║ seats: {4,-27}│                                                            ║
              ║                                   │ Title: {6,-52}║
              ║ price: {5,-27}│                                                            ║
              ║                                   │                                                            ║
              ╚════════════════════════════════════════════════════════════════════════════════════════════════╝"
        };
        public Ticket(int id) 
        {
            this.id = id;   
        }


        // Gettery & Settery
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public int Price
        {
            get { return price; }
            set { price = value; }
        }


        // Metoda obliczająca cenę biletu
        public double CalculatePrice(double price, int seatsCount)
        {
            return price * seatsCount;
        }


        // Metoda zapisująca bilet do pliku tekstowego
        public void SaveTicketToFile(Movie movie, string seats)
        {
            string title = movie.Title.Replace(" ", "_");
            string directoryPath = "../../../Database/Tickets";
            string fileName = id + "_" + title + ".txt";
            string filePath = Path.Combine(directoryPath, fileName);

            StreamWriter sw = new StreamWriter(filePath);

            double price = Math.Round(CalculatePrice(movie.Price, seats.Count(x => x == ',') + 1), 2);
            string finalPrice = "$"+ price.ToString();
            string duration = movie.Duration + "h";
            string date = movie.Date.ToString("dd/MM/yyyy HH:mm");
            string room = movie.Room.Number.ToString();

            // Formatowanie szablonu biletu (wstawienie odpowiednich danych)
            string formattedTicket = string.Format(
                string.Join(Environment.NewLine, ticketTemplate),
                id,
                date,
                duration,
                room,
                seats,
                finalPrice,
                movie.Title
            );
            sw.WriteLine( formattedTicket );    // Zapis biletu do pliku
            sw.Close();
        }
        

        // Metoda usuwająca plik tekstowy z biletem
        public void DestroyTicket(string title)
        {
            title = title.Replace(" ", "_");
            string directoryPath = "../../../Database/Tickets";
            string fileName = id + "_" + title + ".txt";
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


        // Metoda wstawiająca dane do szablonu i zwracająca gotowy szablon biletu
        public string[] RenderTemplate(Reservation res)
        {
            // Formatowanie danych
            string title = res.Movie.Title;
            string date = res.Movie.Date.ToString("dd/MM/yyyy HH:mm");
            string duration = res.Movie.Duration + "h";
            string room = res.Movie.Room.Number.ToString();
            string seats = string.Join(",", res.Seats);
            double price = Math.Round(CalculatePrice(res.Movie.Price, seats.Count(x => x == ',') + 1), 2);
            string finalPrice = "$" + price.ToString();

            string fileName = "Tickets/"+id + "_" + title + ".txt";

            // Wstawienie danych do odpowiednich miejsc w szablonie
            string formattedTicket = string.Format(
                string.Join(Environment.NewLine, ticketTemplate),
                id,
                date,
                duration,
                room,
                seats,
                price,
                title
            );

            string[] temp = new string[2];
            temp[0] = formattedTicket;
            temp[1] = fileName;
            return temp;    // Zwrócenie uzupełnionego szablonu biletu
        }
    }
}