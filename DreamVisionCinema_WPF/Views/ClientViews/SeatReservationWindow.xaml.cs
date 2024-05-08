using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DreamVisionCinema_WPF.Views.ClientViews
{
    /// <summary>
    /// Interaction logic for SeatReservationWindow.xaml
    /// </summary>
    public partial class SeatReservationWindow : Window
    {
        private DispatcherTimer timer;
        public SeatReservationWindow()
        {
            InitializeComponent();
            GenerateSeats(10, 10);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop(); // Zatrzymaj timer

            // Ukryj GIF
            gifImage.Visibility = Visibility.Collapsed;

            // Pokaż obraz
            finalImage.Visibility = Visibility.Visible;

            // Dostosuj rozmiar okna do obrazu
            //this.Width = 300; // Przykładowa szerokość
            //this.Height = 300; // Przykładowa wysokość
        }


        private void GenerateSeats(int rows, int cols)
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    // Utworzenie nowego przycisku
                    var seatButton = new Button
                    {
                        Margin = new Thickness(5),
                        Background = Brushes.Green // Domyślny kolor wolnego miejsca
                    };

                    // Ustawienie zawartości przycisku jako StackPanel
                    var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
                    var image = new Image
                    {
                        Source = new BitmapImage(new Uri("C:\\Users\\kubap\\OneDrive\\Pulpit\\6_SEMESTR_MATERIAŁY\\WPF\\ProjektWPF\\DreamVisionCinema_WPF\\DreamVisionCinema_WPF\\Assets\\couch-solid.png")), // Ścieżka do obrazka
                        Height = 15, // Wysokość obrazka
                        Width = 15  // Szerokość obrazka
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

                    seatButton.Click += SeatButton_Click;
                    seatsPanel.Children.Add(seatButton);
                }
            }
        }

        private void SeatButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            // Przełączanie koloru tła przycisku
            if (button.Background == Brushes.Green)
            {
                button.Background = Brushes.Red;
            }
            else
            {
                button.Background = Brushes.Green;
            }
            UpdateSelectedSeatsDisplay();
        }
        private void UpdateSelectedSeatsDisplay()
        {
            var selectedSeats = new List<string>();
            foreach (Button seat in seatsPanel.Children)
            {
                if (seat.Background == Brushes.Red)
                {
                    // Pobieranie numeru miejsca z Label wewnątrz StackPanel, który jest zawartością przycisku
                    StackPanel stack = seat.Content as StackPanel;
                    if (stack != null && stack.Children.Count > 1 && stack.Children[1] is Label label)
                    {
                        selectedSeats.Add(label.Content.ToString());
                    }
                }
            }
            selectedSeatsText.Text = "Wybrane miejsca: " + string.Join(", ", selectedSeats);
        }

        private void BuyTicket_Click(object sender, RoutedEventArgs e)
        {
            blurEffect.Radius = 5;  // Ustaw promień rozmazywania
            purchasePopup.IsOpen = true; // Otwiera Popup
        }

        private void SubmitTicketPurchase(object sender, RoutedEventArgs e)
        {
            purchasePopup.IsOpen = false;
            gifImage.Visibility = Visibility.Visible;
            finalImage.Visibility = Visibility.Collapsed;

            if (gifImage.Source != null && gifImage.Source.Width > 0 && gifImage.Source.Height > 0)
            {
                //this.Width = gifImage.Source.Width;
                //this.Height = gifImage.Source.Height;
            }
            else
            {
                // Ustaw domyślne wartości, jeśli Source jest null lub nie ma wymiarów
                //this.Width = 300; // Przykładowa szerokość
                //this.Height = 300; // Przykładowa wysokość
            }

            timer.Start();
        }




        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

