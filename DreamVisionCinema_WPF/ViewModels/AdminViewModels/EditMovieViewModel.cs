using DreamVisionCinema_WPF.ViewModels.ClientViewModels;
using System.Windows.Input;

namespace DreamVisionCinema_WPF.ViewModels.AdminViewModels
{
    public class EditMovieViewModel : BaseViewModel
    {
        private static EditMovieViewModel _instance = null;
        public ICommand DragMoveCommand => base.DragCommand;
        public EditMovieViewModel()
        {

        }
        public static EditMovieViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EditMovieViewModel();
                    return _instance;
                }
                else
                    return _instance;
            }
        }
    }
}
