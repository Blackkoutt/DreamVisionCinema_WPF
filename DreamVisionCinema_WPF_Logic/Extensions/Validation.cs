using DreamVisionCinema_WPF_Logic.Enums;
using DreamVisionCinema_WPF_Logic.Exceptions;

namespace DreamVisionCinema_WPF_Logic.Extensions
{

    // Klasa zawierająca statyczne metody służące do walidacji poprawności danych
    public class Validation
    { 

        // Metoda sprawdzająca czy czas trwania filmu jest prawidłowy
        public static void IsDurationCorrect(string duration)
        {
            // Jeśli czas trwania nie posiada dwukropka wyrzuć wyjątek
            if (!duration.Contains(":"))
            {
                throw new IncorrectParametrException("Czas trwania powinien być podany w formacie: H:MM lub HH:MM");
            }

            for (int i = 0; i < duration.Length; i++)
            {
                // Jeśli czas trwania zawiera cokolwiek innego niż ":" i liczby wyrzuć wyjątek
                if (duration[i] != ':' && !char.IsDigit(duration[i]))
                {
                    throw new IncorrectParametrException("Czas trwania powinien być podany w formacie: H:MM lub HH:MM.");
                }
            }

            // Jeśli czas trwania jest zerowy wyrzuć wyjątek
            bool zeroDuration = true;
            for (int i = 0; i < duration.Length; i++)
            {
                if (duration[i] != ':' && duration[i] != '0')
                {
                    zeroDuration = false;
                    break;
                }
            }
            if (zeroDuration)
            {
                throw new IncorrectParametrException("Czas trwania nie może być zerowy");
            }
        }


        // Metoda sprawdzająca czy numer sali jest poprawny
        public static bool IsValidRoomNumber(int? number)
        {
            // Jeśli numer sali jest z zakresu (0,100] to jest poprawny
            if(number>0 && number <= (int)Rooms.COUNT)
            {
                return true;
            }
            return false;   
        }
    }
}