using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Enums;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF.ViewModels.AdminViewModels;
using DreamVisionCinema_WPF.Views.AdminViews;
using DreamVisionCinema_WPF_Logic.Exceptions;
using DreamVisionCinema_WPF_Logic.Interfaces;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using DreamVisionCinema_WPF_Logic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using Unity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using QRCoder;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace DreamVisionCinema_WPF.ViewModels.ClientViewModels
{
    public class ClientReservationListViewModel : BaseViewModel
    {
        private static ClientReservationListViewModel _instance = null;

        public ObservableCollection<Reservation> SessionReservations { get; set; }
        //public static List<Reservation> SessionReservations { get; set; } = new List<Reservation>();

        public ICommand SearchReservationListCommand { get; set; }
        public ICommand DownloadTicketCommand { get; set; }
        private string? searchValue;
        private ResourceDictionary resourceDictionary;


        public ClientReservationListViewModel()
        {
            SessionReservations = new ObservableCollection<Reservation>();
            SearchReservationListCommand = new RelayCommand(SearchReservationList);
            DownloadTicketCommand = new RelayCommand(DownloadTicket);
            resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Styles/Colors.xaml")
            };
        }

        public void LoadOrRefreshReservatonList()
        {
            try
            {
                SessionReservations = new ObservableCollection<Reservation>();
            }
            catch (Exception ex)
            {
                MakeAlert(ex.Message, AlertTypeEnum.Error, true);
                return;
            }
        }

        private void DownloadTicket(object parameter)
        {
            if (parameter is Reservation reservation)
            {
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
                        new FormattedText(reservation.Movie.Title, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, 22, brush),
                    new System.Windows.Point(140, 170) // Adjust the position as needed
                    );

                    drawingContext.DrawText(
                        new FormattedText(reservation.Movie.Date.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, brush),
                        new System.Windows.Point(100, 235) // Adjust the position as needed
                    );

                    drawingContext.DrawText(
                        new FormattedText(reservation.Movie.Price.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, brush),
                        new System.Windows.Point(160, 270) // Adjust the position as needed
                    );

                    drawingContext.DrawText(
                        new FormattedText(reservation.SeatsAsString, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, brush),
                    new System.Windows.Point(130, 305) // Adjust the position as needed
                    );

                    drawingContext.DrawText(
                        new FormattedText(reservation.Movie.Duration.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, brush),
                        new System.Windows.Point(400, 230) // Adjust the position as needed
                    );

                    drawingContext.DrawText(
                        new FormattedText(reservation.Movie.Room.Number.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, fontSize, brush),
                        new System.Windows.Point(320, 267) // Adjust the position as needed
                    );

                    var qrCodeImageSource = CreateQRCode(reservation.Id.ToString(), reservation.Movie.Title);
                    drawingContext.DrawImage(qrCodeImageSource, new Rect(515, 100, 190, 190));
                }

                // Render the visual to a bitmap
                var renderTargetBitmap = new RenderTargetBitmap(bitmapImage.PixelWidth, bitmapImage.PixelHeight, 96, 96, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(drawingVisual);

                // Save the bitmap to a file
                /**/

                var pngImage = new PngBitmapEncoder();
                pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

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
        private void SearchReservationList(object parameter)
        {
            string search_value = (SearchValue != null) ? SearchValue : string.Empty;

            if (SessionReservations.Any())
            {
                try
                {
                    var filteredList = SessionReservations.Where(r =>
                        r.Movie.Title.Contains(search_value, StringComparison.OrdinalIgnoreCase) ||
                        r.Movie.Date.ToString("dd-MM-yyyy").Contains(search_value) ||
                        r.Movie.Room.Number.ToString().Contains(search_value) ||
                        r.Ticket.Price.ToString().Contains(search_value) ||
                        r.SeatsAsString.Contains(search_value, StringComparison.OrdinalIgnoreCase)).ToList();

                    if (!filteredList.Any())
                    {
                        throw new CannotFindMatchingReservationException("Nie znaleziono pasujących rezerwacji.");
                    }

                    SessionReservations = new ObservableCollection<Reservation>(filteredList);
                }
                catch (CannotFindMatchingReservationException CFMRE)
                {
                    MakeAlert(CFMRE.Message, AlertTypeEnum.Info, true);
                    return;
                }
                catch (Exception ex)
                {
                    MakeAlert(ex.Message, AlertTypeEnum.Error, true);
                    return;
                }
            }
            else
            {
                MakeAlert("Lista filmów jest pusta", AlertTypeEnum.Info, true);
            }
        }

        public static ClientReservationListViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ClientReservationListViewModel();
                    return _instance;
                }
                else
                    return _instance;
            }
        }

        public string? SearchValue
        {
            get { return searchValue; }
            set { searchValue = value; }
        }
    }
}
