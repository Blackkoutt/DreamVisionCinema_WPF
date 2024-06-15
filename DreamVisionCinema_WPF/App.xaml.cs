using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF_Logic.Interfaces;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using DreamVisionCinema_WPF_Logic.Model;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Forms.Design;
using Unity;

namespace DreamVisionCinema_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUnityContainer _container = DIContainer.GetContainer();

            _container.RegisterType<IMovieRepository, MovieRepository>();
            _container.RegisterType<IReservationRepository, ReservationRepository>();
        }
    }

}
