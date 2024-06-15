using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF_Logic.Interfaces;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using DreamVisionCinema_WPF_Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DreamVisionCinema_WPF.Views.ClientViews.ViewModel
{
    public class SeatReservationViewModel : BaseViewModel
    {
        private DispatcherTimer timer;
        private ResourceDictionary resourceDictionary;
        public ICommand GenerateSeatsCommand { get; private set; }
        public ICommand SeatButtonCommand { get; private set; }
        public ICommand BuyTicketCommand { get; private set; }
        public ICommand SubmitTicketPurchaseCommand { get; private set; }


        public Image GifImage { get; set; }
        public Image FinalImage { get; set; }
        public Popup PurchasePopup { get; set; }
        public WrapPanel SeatsPanel { get; set; }
        public TextBlock SelectedSeatsText { get; set; }
        public BlurEffect BlurEffect { get; set; }
        public Grid GifPanel { get; set; }

        public Movie movie { get; set; }
        ReservationRepository reservationRepository { get; set; }
        MovieRepository movieRepository { get; set; }
        List<string> selectedSeats;
        List<string> unavailableSeats; 
        List<Reservation> movieReservations { get; set; }

        public SeatReservationViewModel(Movie movie)
        {
            GenerateSeatsCommand = new RelayCommand(o => GenerateSeats(10, 10));
            SeatButtonCommand = new RelayCommand(SeatButton_Click);
            BuyTicketCommand = new RelayCommand(BuyTicket_Click);
            SubmitTicketPurchaseCommand = new RelayCommand(SubmitTicketPurchase);

            this.movie = movie;

            resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Styles/Colors.xaml")
            };


            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
            movieRepository = new MovieRepository();
            movieRepository.ReadMoviesFromFile();
            reservationRepository = new ReservationRepository(movieRepository);
            reservationRepository.ReadReservationsFromFile();
            movieReservations = reservationRepository.GetReservationForMovie(movie);
            unavailableSeats = new List<string>();
            selectedSeats = new List<string>();
            SeeUnavailableSeatsForMovie();
        }

        private void SeeUnavailableSeatsForMovie()
        {
            foreach (var reservation in movieReservations)
            {
                foreach(var seat in reservation.Seats)
                {
                    if (!seat.IsAvailable)
                    {
                        unavailableSeats.Add(seat.ToString());
                    }
                }
            }
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
                    if (unavailableSeats.Contains((row * cols + col + 1).ToString()))
                    {
                        seatButton.Background = Brushes.Gray;
                        ToolTip toolTip = new ToolTip
                        {
                            Content = "To miejsce jest już zajęte",
                            Background = (Brush)resourceDictionary["secondaryBackground"],
                            Margin = new Thickness(5),
                            Padding = new Thickness(5),
                            Foreground= Brushes.White,

                    };
                        ToolTipService.SetInitialShowDelay(seatButton, 200);
                        seatButton.ToolTip = toolTip;
                    }

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
                    if (seat.Content is StackPanel stack &&
                        stack.Children[1] is Label label &&
                        int.Parse(label.Content.ToString()) == seatNumber &&
                        !(unavailableSeats.Contains(label.Content.ToString())))
                    {
                        if(selectedSeats.Count < 5 || seat.Background == Brushes.Red)
                        seat.Background = seat.Background == Brushes.Green ? Brushes.Red : Brushes.Green;
                        else
                        {
                            MessageBox.Show("Możesz wybrać maksymalnie 5 miejsc!","Osiągnięto limit miejsc",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            
                        }
                        break;
                    }
                }
                UpdateSelectedSeatsDisplay();
            }
        }

        private void UpdateSelectedSeatsDisplay()
        {
            var tempSelectedSeats = new List<string>();
            foreach (Button seat in SeatsPanel.Children)
            {
                if (seat.Background == Brushes.Red)
                {
                    if (seat.Content is StackPanel stack && stack.Children.Count > 1 && stack.Children[1] is Label label && !(unavailableSeats.Contains(label.Content.ToString())))
                    {
                        tempSelectedSeats.Add(label.Content.ToString());
                    }
                }
            }
            SelectedSeatsText.Text = "Wybrane miejsca: " + string.Join(", ", tempSelectedSeats);
            selectedSeats = tempSelectedSeats;
        }

        private void BuyTicket_Click(object parameter)
        {
            BlurEffect.Radius = 5;
            PurchasePopup.IsOpen = true;
            SetIsHitTestVisibleForElements(false);
        }

        private async void SubmitTicketPurchase(object parameter)
        {
            PurchasePopup.IsOpen = false;
            SetIsHitTestVisibleForElements(true);

            // Pokaż GIF
            BlurEffect.Radius = 0;
            GifImage.Visibility = Visibility.Visible;
            GifPanel.Background = (Brush)resourceDictionary["primaryBackground"];

            // Wykonaj operację na wątku innym niż wątek interfejsu użytkownika
            await Task.Delay(3000); // lub dowolny czas oczekiwania
                                   // Tutaj możesz wykonać operacje na wątku w tle, na przykład zapis danych, pobieranie itp.

            // Ukryj GIF
            GifImage.Visibility = Visibility.Collapsed;

            // Pozostała część Twojej metody
            FinalImage.Visibility = Visibility.Visible;
            await Task.Delay(3000);
            Reservation reservation = reservationRepository.GetLastReservation();
            reservationRepository.MakeReservation((reservation.Id + 1).ToString(), movie.Id.ToString(), selectedSeats);
            reservationRepository.SaveReservationsToFile();
        }


        private void SetIsHitTestVisibleForElements(bool isVisible)
        {
            // Przechodzenie przez dzieci kontenera, np. Grid, StackPanel, itp.
            foreach (var child in ((Panel)PurchasePopup.Parent).Children)
            {
                if (child != PurchasePopup)
                {
                    // Ustawienie właściwości IsHitTestVisible na isVisible dla każdego elementu
                    ((UIElement)child).IsHitTestVisible = isVisible;
                }
            }
        }

    }
}
