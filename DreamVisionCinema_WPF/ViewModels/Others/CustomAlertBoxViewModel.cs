using DreamVisionCinema_WPF.Enums;
using DreamVisionCinema_WPF.Observable;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace DreamVisionCinema_WPF.ViewModels.Others
{
    public class CustomAlertBoxViewModel : ObservableObject
    {
        public ICommand CloseCommand { get; set; }
        private string message;
        private string? icon;
        private bool isTimerEnabled;
        private DispatcherTimer timer;
        private AlertActionEnum action;
        private double x;
        private double width, height;
        private Window alertBox;
        private Brush backgroudColor;

        public CustomAlertBoxViewModel(string message, AlertTypeEnum type, bool isTimerEnabled, double width, double height, Window alertBox)
        {
            this.message = message;
            
            CloseCommand = new RelayCommand(CloseAlertBox);
            this.isTimerEnabled = isTimerEnabled;
            this.width = width;
            this.height = height;
            this.alertBox = alertBox;
            timer = new DispatcherTimer();
            ShowAlert(type);
        }
        private void ShowAlert(AlertTypeEnum type)
        {
            alertBox.Opacity = 0.0;
   
            string alertName;
            for (int i = 1; i < 10; i++)
            {
                alertName = "alert" + i.ToString();

                // Ustalenie punktu w którym ma pojawić się powiadomienie
                if (AlertWindowManager.GetAlert(alertName) == null)
                {
                    alertBox.Name = alertName;
                    AlertWindowManager.AddAlert(alertName, alertBox);
                    x = SystemParameters.WorkArea.Width - width + 5;
                    alertBox.Left = x;
                    alertBox.Top = SystemParameters.WorkArea.Height - height * i;
                    break;
                }
            }
            x = SystemParameters.WorkArea.Width - width - 5;

            MapTypeToAlertDesign(type);

            alertBox.Show();

            if (isTimerEnabled)
            {
                action = AlertActionEnum.Start;
                timer.Interval = TimeSpan.FromMilliseconds(1);
                timer.Tick += timer_Tick;
                timer.Start();
            }
            else
            {
                alertBox.Opacity = 1.0;
                int a = 1;
            }
        }

        private void timer_Tick(object? sender, EventArgs e)
        {
            switch (action)
            {
                case AlertActionEnum.Wait:
                    {
                        timer.Interval = TimeSpan.FromSeconds(5);
                        action = AlertActionEnum.Close;
                        break;
                    }

                case AlertActionEnum.Start:
                    {
                        timer.Interval = TimeSpan.FromMilliseconds(1);
                        
                        if (alertBox.Left > x)
                        {
                            alertBox.Left--;
                            alertBox.Opacity += 0.1;
                        }
                        if (alertBox.Opacity >= 1.0)
                        {
                            action = AlertActionEnum.Wait;
                        }
                        /*else
                        {
                            
                        }*/
                        break;
                    }

                case AlertActionEnum.Close:
                    {
                        timer.Interval = TimeSpan.FromMilliseconds(1);
                        alertBox.Opacity -= 0.1;
                        alertBox.Left -= 3;
                        if (alertBox.Opacity <= 0.0)
                        {
                            timer.Stop();
                            AlertWindowManager.RemoveAlert(alertBox.Name);
                            alertBox.Close();
                        }
                        break;
                    }
            }
        }

        private void MapTypeToAlertDesign(AlertTypeEnum type)
        {
            switch (type)
            {
                case AlertTypeEnum.Success:
                    {
                        Icon = "Solid_Check";
                        BackgroundColor = new SolidColorBrush(Colors.SeaGreen);
                        break;
                    }
                case AlertTypeEnum.Error:
                    {
                        Icon = "Solid_ExclamationTriangle";
                        BackgroundColor = new SolidColorBrush(Colors.DarkRed);
                        break;
                    }
                case AlertTypeEnum.Info:
                    {
                        Icon = "Solid_InfoCircle";
                        BackgroundColor = new SolidColorBrush(Colors.RoyalBlue);
                        break;
                    }
            }
        }

        private void CloseAlertBox(object parameter)
        {
            //if (parameter is Window window)
            //{
                timer.Interval = TimeSpan.FromMilliseconds(1);
                action = AlertActionEnum.Close;
                if (!isTimerEnabled)
                {
                    timer.Start();
                }
                //window.Close();
           // }
        }
        public Brush BackgroundColor
        {
            get { return backgroudColor; }
            set
            {
                backgroudColor = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }
        public string? Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                OnPropertyChanged();
            }
        }
    }
}
