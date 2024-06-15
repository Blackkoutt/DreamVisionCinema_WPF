using DreamVisionCinema_WPF.ViewModels.ClientViewModels;
using System.Windows.Input;

namespace DreamVisionCinema_WPF.ViewModels.AdminViewModels
{
    public class DeleteMessageViewModel : BaseViewModel
    {
        private static DeleteMessageViewModel _instance = null;
        public ICommand DragMoveCommand => base.DragCommand;
        public DeleteMessageViewModel()
        {

        }
        public static DeleteMessageViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DeleteMessageViewModel();
                    return _instance;
                }
                else
                    return _instance;
            }
        }
    }
}
