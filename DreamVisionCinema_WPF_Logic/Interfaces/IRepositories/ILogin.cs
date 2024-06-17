namespace DreamVisionCinema_WPF_Logic.Interfaces.IRepositories
{
    public interface ILogin
    {
        bool SignIn(string login, string password);
        void HashPassword(string login, string password);
    }
}
