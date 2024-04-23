namespace DreamVisionCinema_WPF_Logic.Model
{
    public class Movie
    {
        // Każdy film posiada id, tytuł, datę seansu (DD.MM.YYYY HH:MM), cenę, czas trwania i salę w której jest pokazywany
        private int id;
        private string title;
        private DateTime date;
        private double price;
        private string duration;
        private Room room;
        public Movie(int id, string title, DateTime date, double price, string duration, Room room)
        {
            this.id = id;
            this.title = title;
            this.date = date;
            this.price = price;
            this.duration = duration;
            this.room = room;
        }

        // Gettery & Settery
        public int Id
        {
            get { return id; }
        }
        public string Title { 
            get { return this.title; }
            set { this.title = value; }
        }
        public DateTime Date { 
            get { return this.date; }
            set { this.date = value; }
        }
        public double Price
        {
            get { return this.price; }
            set { this.price = value; }
        }
        public string Duration
        {
            get { return this.duration; }
            set { this.duration = value; }
        }
        public Room Room {
            get { return this.room; }
            set {   this.room = value; }
        }  
    }
}