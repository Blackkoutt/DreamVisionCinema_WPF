using DreamVisionCinema_WPF.Observable;
using System;
using System.Windows;
using System.Windows.Input;

namespace DreamVisionCinema_WPF.Views.ClientViews.ViewModel
{
    public class BaseViewModel
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
