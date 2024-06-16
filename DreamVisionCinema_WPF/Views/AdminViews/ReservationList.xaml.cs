using DreamVisionCinema_WPF_Logic.Model;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DreamVisionCinema_WPF.Views.AdminViews
{
    /// <summary>
    /// Logika interakcji dla klasy ReservationList.xaml
    /// </summary>
    public partial class ReservationList : UserControl
    {
        public ReservationList()
        {
            InitializeComponent();

            Movie m1 = new Movie(1, "Najnowszy film bardzo dobry", DateTime.Now, 20.99, "1:30h", new Room(1, 100),"a", "a", "a");
            Movie m2 = new Movie(2, "Film 2", DateTime.Now, 20.99, "1:30h", new Room(1, 100), "a", "a", "a");
            List <Seat> seats1 = new List<Seat>();
            seats1.Add(new Seat(1));
            seats1.Add(new Seat(2));
            seats1.Add(new Seat(3));
            seats1.Add(new Seat(4));

            List<Seat> seats2 = new List<Seat>();
            seats2.Add(new Seat(5));
            seats2.Add(new Seat(6));
            seats2.Add(new Seat(7));
            seats2.Add(new Seat(8));
            seats2.Add(new Seat(9));

            List<Reservation> res = new List<Reservation>();

            res.Add(new Reservation(1, m1, seats1));
            res.Add(new Reservation(2, m2, seats2));

            res[0].Ticket.Price = (int)res[0].Ticket.CalculatePrice(res[0].Movie.Price, res[0].Seats.Count);
            res[1].Ticket.Price = (int)res[1].Ticket.CalculatePrice(res[1].Movie.Price, res[1].Seats.Count);
            reservationsList.ItemsSource = res;
        }
    }
}
