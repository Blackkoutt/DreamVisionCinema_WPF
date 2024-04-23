using DreamVisionCinema_WPF_Logic.Enums;
using DreamVisionCinema_WPF_Logic.Exceptions;
namespace DreamVisionCinema_WPF_Logic.Model
{
    public class Room
    {
        private int number; // Numer sali
        private Seat[] seats;

        public Room(int number, int NumberOfSeats) {
            this.number = number;
            seats = new Seat[NumberOfSeats];
            InitSeats();    // Inicjalizacja miejsc
        }


        // Gettery & Settery
        public int Number {
            get {  return number; }
            set { this.number = value; }
        }
        public Seat[] Seats
        {
            get { return seats; }
        }


        // Metoda sprawdzająca czy podane miejsca są wolne
        public void CheckSeatsAvailable(List<int> SeatsNumbers)
        {
            List<int> UnavailableSeats=new List<int>();
            foreach (int seat_nr in SeatsNumbers)
            {
                // Sprawdzenie poprawności podanych argumentów
                if(seat_nr<1 || seat_nr > (int)Rooms.NUMBER_OF_SEATS)
                {
                    throw new NoSeatWithGivenNumberException("Brak miejsca o podanym numerze.");
                }
                // Jeśli miejsce jest zajęte dodaj do listy miejsc zajętych
                if(!GetSeat(seat_nr).IsAvailable)
                {
                    UnavailableSeats.Add(seat_nr);
                }
            }
            // Jeśli lista miejsc zajętych zawiera cokolwiek wyrzuć wyjątek
            if (UnavailableSeats.Any())
            {
                string unavailableSeatsMessage = string.Join(", ", UnavailableSeats);
                throw new SeatIsNotAvailableException($"Wybrane miejsca: {unavailableSeatsMessage} są zajęte!");
            }
        }
        

        // Metoda pobierająca miejsce o danym numerze
        private Seat GetSeat(int seat_nr) 
        {
            return seats[seat_nr-1]; // Numery miejsc są numerowane od 1 do 100 a indexy od 0 do 99
        }
        

        // Metoda oznaczająca miejsca jako zajęte i zwracająca te miejsca
        public List<Seat> MarkSeatsAsReservatedAndGet(List<int> SeatsNumbers)
        {
            List<Seat> Seats = new List<Seat>();
            foreach (int seat_nr in SeatsNumbers)
            {
                Seat seat = GetSeat(seat_nr);
                seat.IsAvailable = false;
                Seats.Add(seat);
            }
            return Seats;
        }


        // Metoda oznaczająca miejsca jako wolne
        public void MarkSeatsAsAvailable(List <Seat> Seats)
        {
            foreach (Seat seat in Seats)
            {
                //Seat seat = GetSeat(seat_nr);
                seat.IsAvailable = true;
            }
        }


        // Metoda inicjująca miejsca
        private void InitSeats()
        {
            for(int i = 0; i < seats.Length; i++)
            {
                seats[i] = new Seat(i+1);
            }
        }
    }
}