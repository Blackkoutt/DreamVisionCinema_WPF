using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using DreamVisionCinema_WPF.Views.ClientViews.ViewModel;

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
