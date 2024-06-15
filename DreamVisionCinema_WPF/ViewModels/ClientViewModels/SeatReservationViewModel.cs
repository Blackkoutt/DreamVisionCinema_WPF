using DreamVisionCinema_WPF.Observable;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DreamVisionCinema_WPF.ViewModels.ClientViewModels
{
    public class SeatReservationViewModel : BaseViewModel
    {
        private DispatcherTimer timer;

        public ICommand GenerateSeatsCommand { get; private set; }
        public ICommand SeatButtonCommand { get; private set; }
        public ICommand BuyTicketCommand { get; private set; }
        public ICommand SubmitTicketPurchaseCommand { get; private set; }
        public ICommand DragMoveCommand => base.DragCommand;

        public Image GifImage { get; set; }
        public Image FinalImage { get; set; }
        public Popup PurchasePopup { get; set; }
        public WrapPanel SeatsPanel { get; set; }
        public TextBlock SelectedSeatsText { get; set; }
        public BlurEffect BlurEffect { get; set; }

        public SeatReservationViewModel()
        {
            GenerateSeatsCommand = new RelayCommand(o => GenerateSeats(10, 10));
            SeatButtonCommand = new RelayCommand(SeatButton_Click);
            BuyTicketCommand = new RelayCommand(BuyTicket_Click);
            SubmitTicketPurchaseCommand = new RelayCommand(SubmitTicketPurchase);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            GifImage.Visibility = Visibility.Collapsed;
            FinalImage.Visibility = Visibility.Visible;
        }

        private void GenerateSeats(int rows, int cols)
        {
            SeatsPanel.Children.Clear();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var seatButton = new Button
                    {
                        Margin = new Thickness(5),
                        Background = Brushes.Green,
                        Command = SeatButtonCommand,
                        CommandParameter = row * cols + col + 1
                    };

                    var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
                    var image = new Image
                    {
                        Source = new BitmapImage(new Uri("C:\\Users\\kubap\\OneDrive\\Pulpit\\6_SEMESTR_MATERIAŁY\\WPF\\ProjektWPF\\DreamVisionCinema_WPF\\DreamVisionCinema_WPF\\Assets\\couch-solid.png")),
                        Height = 15,
                        Width = 15
                    };
                    var label = new Label
                    {
                        Content = row * cols + col + 1,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Foreground = Brushes.White
                    };

                    stackPanel.Children.Add(image);
                    stackPanel.Children.Add(label);
                    seatButton.Content = stackPanel;

                    SeatsPanel.Children.Add(seatButton);
                }
            }
        }

        private void SeatButton_Click(object parameter)
        {
            if (parameter is int seatNumber)
            {
                foreach (Button seat in SeatsPanel.Children)
                {
                    if (seat.Content is StackPanel stack && stack.Children[1] is Label label && int.Parse(label.Content.ToString()) == seatNumber)
                    {
                        seat.Background = seat.Background == Brushes.Green ? Brushes.Red : Brushes.Green;
                        break;
                    }
                }
                UpdateSelectedSeatsDisplay();
            }
        }

        private void UpdateSelectedSeatsDisplay()
        {
            var selectedSeats = new List<string>();
            foreach (Button seat in SeatsPanel.Children)
            {
                if (seat.Background == Brushes.Red)
                {
                    if (seat.Content is StackPanel stack && stack.Children.Count > 1 && stack.Children[1] is Label label)
                    {
                        selectedSeats.Add(label.Content.ToString());
                    }
                }
            }
            SelectedSeatsText.Text = "Wybrane miejsca: " + string.Join(", ", selectedSeats);
        }

        private void BuyTicket_Click(object parameter)
        {
            BlurEffect.Radius = 5;
            PurchasePopup.IsOpen = true;
        }

        private void SubmitTicketPurchase(object parameter)
        {
            PurchasePopup.IsOpen = false;
            GifImage.Visibility = Visibility.Visible;
            FinalImage.Visibility = Visibility.Collapsed;
            timer.Start();
        }
    }
}
