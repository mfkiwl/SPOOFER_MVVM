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
        private readonly ILogin _iLogin;
        private readonly ISpoofer _spoofer;
        public static CoordinatesContext _context;

        public App()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlServer(CONNECTION_STRING).Options;
            _context = new CoordinatesContext(options);
            _navigationStore = new NavigationStore();
            _iLogin = new ServiceLogin(_context, new NavigationService(_navigationStore, createMapViewModel));
            _marker = new MarkerService(_context);
            _spoofer = new SpooferService(_context, _marker);
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

    }
}