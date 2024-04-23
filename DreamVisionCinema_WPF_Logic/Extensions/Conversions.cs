using DreamVisionCinema_WPF_Logic.Exceptions;

namespace DreamVisionCinema_WPF_Logic.Extensions
{
    // Klasa zawierająca metody statyczne służące do konwersji
    public class Conversions
    {

        // Konwersja string -> int
        public static int TryParseToInt(string value, string msg)
        {
            try
            {
                return int.Parse(value);
            }
            catch
            {
                throw new CannotConvertException(msg);
            }
        }

        // Konwersja string -> DateTime
        public static DateTime TryParseToDateTime(string value, string msg)
        {
            try
            {
                string format = "dd/MM/yyyy HH:mm";
                return DateTime.ParseExact(value, format, null);
            }
            catch
            {
                throw new CannotConvertException(msg);
            }
        }

        // Konwersja string -> double
        public static double TryParseToDouble(string value, string msg)
        {
            value = value.Replace(".", ",");
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                throw new CannotConvertException(msg);
            }
        }
    }
}