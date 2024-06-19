using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF_Logic.Model;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
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
        private ResourceDictionary resourceDictionary;
        public ICommand GenerateSeatsCommand { get; private set; }
        public ICommand SeatButtonCommand { get; private set; }
        public ICommand BuyTicketCommand { get; private set; }
        public ICommand SubmitTicketPurchaseCommand { get; private set; }
        public ICommand SaveTicketImageToFileCommand { get; private set; }
        public ICommand UndoLastSelectionCommand { get; private set; }

        public System.Windows.Controls.Image GifImage { get; set; }
        public System.Windows.Controls.Image FinalImage { get; set; }
        public Popup PurchasePopup { get; set; }
        public WrapPanel SeatsPanel { get; set; }
        public TextBlock SelectedSeatsText { get; set; }
        public BlurEffect BlurEffect { get; set; }
        public Grid GifPanel { get; set; }
        public Button SaveButton { get; set; }

        public Movie movie { get; set; }
        ReservationRepository reservationRepository { get; set; }
        MovieRepository movieRepository { get; set; }
        List<string> selectedSeats;
        List<string> unavailableSeats;
        List<Reservation> movieReservations { get; set; }
        Stack<string> selectedSeatsStack;

        private RenderTargetBitmap ticketImage;

        public SeatReservationViewModel(Movie movie)
        {
            GenerateSeatsCommand = new RelayCommand(o => GenerateSeats(10, 10));
            SeatButtonCommand = new RelayCommand(SeatButton_Click);
            BuyTicketCommand = new RelayCommand(BuyTicket_Click);
            SubmitTicketPurchaseCommand = new RelayCommand(SubmitTicketPurchase);
            UndoLastSelectionCommand = new RelayCommand(UndoLastSelection);

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
            selectedSeatsStack = new Stack<string>();
            SeeUnavailableSeatsForMovie();
            SaveTicketImageToFileCommand = new RelayCommand(SaveTicketImageToFile);
        }

        private void SeeUnavailableSeatsForMovie()
        {
            foreach (var reservation in movieReservations)
            {
                foreach (var seat in reservation.Seats)
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
                        Background = System.Windows.Media.Brushes.Green,
                        Command = SeatButtonCommand,
                        CommandParameter = row * cols + col + 1
                    };
                    if (unavailableSeats.Contains((row * cols + col + 1).ToString()))
                    {
                        seatButton.Background = System.Windows.Media.Brushes.Gray;
                        ToolTip toolTip = new ToolTip
                        {
                            Content = "To miejsce jest już zajęte",
                            Background = (System.Windows.Media.Brush)resourceDictionary["secondaryBackground"],
                            Margin = new Thickness(5),
                            Padding = new Thickness(5),
                            Foreground = System.Windows.Media.Brushes.White,
                        };
                        ToolTipService.SetInitialShowDelay(seatButton, 200);
                        seatButton.ToolTip = toolTip;
                    }

                    var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
                    System.Windows.Controls.Image image = new System.Windows.Controls.Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Assets/couch-solid.png")),
                        Height = 15,
                        Width = 15
                    };
                    var label = new Label
                    {
                        Content = row * cols + col + 1,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Foreground = System.Windows.Media.Brushes.White
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
                        if (selectedSeats.Count < 5 || seat.Background == System.Windows.Media.Brushes.Red)
                        {
                            seat.Background = seat.Background == System.Windows.Media.Brushes.Green ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.Green;
                            if (seat.Background == System.Windows.Media.Brushes.Red)
                            {
                                selectedSeatsStack.Push(label.Content.ToString());
                            }
                            else
                            {
                                selectedSeatsStack = new Stack<string>(selectedSeatsStack.Where(s => s != label.Content.ToString()));
                            }
                        }
                        else
                        {
                            MakeAlert("Możesz wybrać maksynalnie 5 miejsc", Enums.AlertTypeEnum.Error, true);
                        }
                        break;
                    }
                }
                UpdateSelectedSeatsDisplay();
            }
        }

        private void UndoLastSelection(object parameter)
        {
            if (selectedSeatsStack.Any())
            {
                var lastSelectedSeat = selectedSeatsStack.Pop();
                foreach (Button seat in SeatsPanel.Children)
                {
                    if (seat.Content is StackPanel stack &&
                        stack.Children[1] is Label label &&
                        label.Content.ToString() == lastSelectedSeat)
                    {
                        seat.Background = System.Windows.Media.Brushes.Green;
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
                if (seat.Background == System.Windows.Media.Brushes.Red)
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
            if (selectedSeats.Count > 0)
            {
                BlurEffect.Radius = 5;
                PurchasePopup.IsOpen = true;
                SetIsHitTestVisibleForElements(false);
            }
            else
            {
                MakeAlert("Musisz wybrać co najmniej jedno miejsce!", Enums.AlertTypeEnum.Error, true);
            }
        }

        private RenderTargetBitmap GenerateTicketImage(string movieTitle, string seats, string date, string time, string ticketId, string duration, string price, string hall)
        {
            // Load the image
            var imagePath = "pack://application:,,,/Assets/ticket_template_2.jpg";
            var bitmapImage = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));

            // Create a DrawingVisual for combining the image and the text
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                // Draw the image
                drawingContext.DrawImage(bitmapImage, new Rect(0, 0, bitmapImage.PixelWidth, bitmapImage.PixelHeight));

                // Draw the text
                var typeface = new Typeface("Arial");
                var fontSize = 16;
                var brush = (System.Windows.Media.Brush)resourceDictionary["purple_3"];

                drawingContext.DrawText(
                    new FormattedText(movieTitle, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, 22, brush),
                    new System.Windows.Point(140, 170) // Adjust the position as needed
                );

                drawingContext.DrawText(
                    new FormattedText(date, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, brush),
                    new System.Windows.Point(100, 235) // Adjust the position as needed
                );

                drawingContext.DrawText(
                    new FormattedText(price, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, brush),
                    new System.Windows.Point(160, 270) // Adjust the position as needed
                );

                drawingContext.DrawText(
                    new FormattedText(seats, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, brush),
                    new System.Windows.Point(130, 305) // Adjust the position as needed
                );

                drawingContext.DrawText(
                    new FormattedText(duration, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, brush),
                    new System.Windows.Point(400, 230) // Adjust the position as needed
                );

                drawingContext.DrawText(
                    new FormattedText(hall, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, brush),
                    new System.Windows.Point(320, 267) // Adjust the position as needed
                );

                var qrCodeImageSource = CreateQRCode(ticketId, movieTitle);
                drawingContext.DrawImage(qrCodeImageSource, new Rect(515, 100, 190, 190));
            }

            // Render the visual to a bitmap
            var renderTargetBitmap = new RenderTargetBitmap(bitmapImage.PixelWidth, bitmapImage.PixelHeight, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(drawingVisual);

            // Save the bitmap to a file
            /**/
            ticketImage = renderTargetBitmap;
            return renderTargetBitmap;
        }

        private void SaveTicketImageToFile(object parameter)
        {
            var pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(ticketImage));

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                FileName = "MovieTicket.png"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    pngImage.Save(stream);
                }
                MakeAlert("Zapisano bilet do plików", Enums.AlertTypeEnum.Success, true);
            }
        }

        private ImageSource CreateQRCode(string ticket_id, string movie_title)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{ticket_id}_{movie_title}", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    qrCodeImage.Save(memory, ImageFormat.Png);
                    memory.Position = 0;
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    return bitmapImage;
                }
            }
        }

        private async void SubmitTicketPurchase(object parameter)
        {
            PurchasePopup.IsOpen = false;
            SetIsHitTestVisibleForElements(true);

            // Pokaż GIF
            BlurEffect.Radius = 0;
            GifImage.Visibility = Visibility.Visible;
            GifPanel.Background = (System.Windows.Media.Brush)resourceDictionary["primaryBackground"];

            await Task.Delay(3000);

            // Ukryj GIF
            GifImage.Visibility = Visibility.Collapsed;

            var ticketImage = GenerateTicketImage(
                movie.Title,
                string.Join(", ", selectedSeats),
                DateTime.Now.ToString("dd-MM-yyyy"),
                DateTime.Now.ToString("HH:mm"),
                "12345", // Example ticket ID
                "2h 30min", // Example duration
                "20.00 PLN", // Example price
                "1" // Example hall number
            );

            SelectedSeatsText.Text = "";
            SaveButton.Visibility = Visibility.Visible;
            FinalImage.Source = ticketImage;
            FinalImage.Visibility = Visibility.Visible;
            
            Reservation reservation = reservationRepository.GetLastReservation();
            reservationRepository.MakeReservation((reservation.Id + 1).ToString(), movie.Id.ToString(), selectedSeats);
            reservationRepository.SaveReservationsToFile();
            reservation = reservationRepository.GetLastReservation();
            ClientReservationListViewModel.Instance.SessionReservations.Add(reservation);
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
