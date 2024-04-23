namespace DreamVisionCinema_WPF_Logic.Model
{
    public class Seat
    {
        private int number; // Numer miejsca
        private bool isAvailable;   // Czy zajęte
        public Seat(int number) {
            this.number = number;
            isAvailable = true;
        }


        // Gettery & Settery
        public bool IsAvailable
        {
            get { return isAvailable; } 
            set { isAvailable = value; }
        }
        public int Number
        {
            get { return number; }
        }


        // Nadpisana metoda ToString()
        public override string ToString()
        {
            return $"{number}";
        }
    }
}