namespace DreamVisionCinema_WPF_Logic.Model
{
    public class Reservation
    {
        // Każda rezerwacja posiada id, film, miejsca oraz bilet
        private int id;
        private Movie movie;
        private List<Seat> seats;
        private Ticket ticket;
        public Reservation(int id, Movie movie, List<Seat> seats)
        {
            this.id = id;
            this.movie = movie;
            this.seats = seats;
            ticket = new Ticket(id);
        }

        // Gettery
        public int Id { get { return id; } }
        public Movie Movie { get { return movie; } }
        public List<Seat> Seats { get { return seats; } }
        public Ticket Ticket { get { return ticket; } }
    }
}