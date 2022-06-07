using Microsoft.EntityFrameworkCore;
using Spoofer.Data;
using Spoofer.Services.Marker;
using Spoofer.Services.Navigation;
using Spoofer.Services.Spoofer;
using Spoofer.Services.User;
using Spoofer.Stores;
using Spoofer.ViewModels;
using System.Windows;

namespace Spoofer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string CONNECTION_STRING = "Server=(localdb)\\mssqllocaldb;Database=Coordinates;Trusted_Connection=True;";
        private readonly NavigationStore _navigationStore;
        private readonly IMarkerService _marker;
        private readonly IMarkerService _tableMarker;
        private readonly ILogin _iLogin;
        private readonly ISpooferService _spoofer;
        public static CoordinatesContext _context;

        public App()
        {

            DbContextOptions options = new DbContextOptionsBuilder().UseSqlServer(CONNECTION_STRING).Options;
            _context = new CoordinatesContext(options);
            _navigationStore = new NavigationStore();
            _marker = new MarkerService(_context, new NavigationService(_navigationStore, createTransmitViewModel));
            _tableMarker = new MarkerService(_context, new NavigationService(_navigationStore, createMapViewModel));
            _spoofer = new SpooferService(_context, _marker);
            _iLogin = new ServiceLogin(_context, new NavigationService(_navigationStore, createMapViewModel), _spoofer);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _navigationStore.BaseViewModel = createAccountViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };

            MainWindow.Show();
        }

        private MapViewModel createMapViewModel()
        {
            return new MapViewModel(_marker, _spoofer);
        }

        private AccountViewModel createAccountViewModel()
        {
            return new AccountViewModel(_iLogin);
        }
        private TransmitInOrderViewModel createTransmitViewModel()
        {
            return new TransmitInOrderViewModel(_tableMarker, _spoofer);
        }

    }
}