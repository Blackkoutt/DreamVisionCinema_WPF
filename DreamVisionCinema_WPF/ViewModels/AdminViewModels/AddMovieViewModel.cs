using DreamVisionCinema_WPF.ViewModels.ClientViewModels;
using System.Windows.Input;

namespace DreamVisionCinema_WPF.ViewModels.AdminViewModels
{
    public class AddMovieViewModel : BaseViewModel
    {
        private static AddMovieViewModel _instance = null;
        public ICommand DragMoveCommand => base.DragCommand;
        public AddMovieViewModel() 
        {

        }
        public static AddMovieViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AddMovieViewModel();
                    return _instance;
                }
                else
                    return _instance;
            }
        }
    }
}
