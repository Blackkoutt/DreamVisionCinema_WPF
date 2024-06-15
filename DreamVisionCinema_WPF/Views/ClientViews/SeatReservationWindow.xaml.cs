using DreamVisionCinema_WPF.ViewModels.ClientViewModels;
using System.Windows;

namespace DreamVisionCinema_WPF.Views.ClientViews
{
    public partial class SeatReservationWindow : Window
    {
        public SeatReservationWindow()
        {
            InitializeComponent();
            var viewModel = new SeatReservationViewModel
            {
                GifImage = gifImage,
                FinalImage = finalImage,
                PurchasePopup = purchasePopup,
                SeatsPanel = seatsPanel,
                SelectedSeatsText = selectedSeatsText,
                BlurEffect = blurEffect
            };
            DataContext = viewModel;
            viewModel.GenerateSeatsCommand.Execute(null); // Generate seats on initialization
        }
    }
}
