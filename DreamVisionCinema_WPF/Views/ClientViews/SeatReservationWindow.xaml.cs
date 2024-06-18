using DreamVisionCinema_WPF.ViewModels.ClientViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using DreamVisionCinema_WPF_Logic.Model;

namespace DreamVisionCinema_WPF.Views.ClientViews
{
    public partial class SeatReservationWindow : Window
    {
        public SeatReservationWindow(Movie movie)
        {
            InitializeComponent();
            var viewModel = new SeatReservationViewModel(movie)
            {
                GifImage = gifImage,
                FinalImage = finalImage,
                PurchasePopup = purchasePopup,
                SeatsPanel = seatsPanel,
                SelectedSeatsText = selectedSeatsText,
                BlurEffect = blurEffect,
                GifPanel = gifPanel,
                SaveButton = saveButton,
            };
            DataContext = viewModel;
            viewModel.GenerateSeatsCommand.Execute(null); // Generate seats on initialization
        }
    }
}
