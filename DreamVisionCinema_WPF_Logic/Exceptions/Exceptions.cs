namespace DreamVisionCinema_WPF_Logic.Exceptions
{
    public class BadAttributeException : Exception
    {
        public BadAttributeException(string message) : base(message) { }
    }


    public class CannotConvertException : Exception
    {
        public CannotConvertException(string message) : base(message) { }
    }


    public class CannotDestroyTicketException : Exception
    {
        public CannotDestroyTicketException(string message) : base(message) { }
    }


    public class CannotFindMatchingMovieException : Exception
    {
        public CannotFindMatchingMovieException(string message) : base(message) { }
    }


    public class CannotReadFileException : Exception
    {
        public CannotReadFileException(string message) : base(message) { }
    }

    public class FileSyntaxException : Exception
    {
        public FileSyntaxException(string message) : base(message) { }
    }


    public class IncorrectParametrException : Exception
    {
        public IncorrectParametrException(string message) : base(message) { }
    }


    public class ListIsEmptyException : Exception
    {
        public ListIsEmptyException(string message) : base(message) { }
    }


    public class MovieListIsEmptyException : Exception
    {
        public MovieListIsEmptyException(string message) : base(message) { }
    }


    public class MoviesCollisionException : Exception
    {
        public MoviesCollisionException(string message) : base(message) { }
    }


    public class NoMovieWithGivenIdException : Exception
    {
        public NoMovieWithGivenIdException(string message) : base(message) { }
    }


    public class NoReservationWithGivenIdException : Exception
    {
        public NoReservationWithGivenIdException(string message) : base(message) { }
    }


    public class NoRoomWithGivenNumberException : Exception
    {
        public NoRoomWithGivenNumberException(string message) : base(message) { }
    }


    public class NoSeatWithGivenNumberException : Exception
    {
        public NoSeatWithGivenNumberException(string message) : base(message) { }
    }


    public class ReservationListIsEmptyException : Exception
    {
        public ReservationListIsEmptyException(string message) : base(message) { }
    }


    public class SeatIsNotAvailableException : Exception
    {
        public SeatIsNotAvailableException(string message) : base(message) { }
    }
}