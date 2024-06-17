using DreamVisionCinema_WPF.Enums;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.Views.Others;
using System.Windows;
using System.Windows.Input;

namespace DreamVisionCinema_WPF.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        public ICommand MinimizeCommand { get; private set; }
        public ICommand MaximizeCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        public ICommand DragCommand {  get; private set; }

        public BaseViewModel()
        {
            MinimizeCommand = new RelayCommand(MinimizeWindow);
            MaximizeCommand = new RelayCommand(MaximizeWindow);
            CloseCommand = new RelayCommand(CloseWindow);
            DragCommand = new RelayCommand(DragWindow);
        }

        private void MinimizeWindow(object parameter)
        {
            if (parameter is Window window)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void MaximizeWindow(object parameter)
        {
            if (parameter is Window window)
            {
                if (window.WindowState != WindowState.Maximized)
                {
                    window.WindowState = WindowState.Maximized;
                }
                else
                {
                    window.WindowState = WindowState.Normal;
                }
            }
        }
        protected void MakeAlert(string message, AlertTypeEnum type, bool isTimerEnabled)
        {
            new CustomAlertBox(message, type, isTimerEnabled);
        }
        private void CloseWindow(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }
        private void DragWindow(object parameter)
        {
            if(parameter is Window window)
            {
                window.DragMove();
            }
        }
    }
}
