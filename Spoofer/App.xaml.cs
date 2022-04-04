using Microsoft.EntityFrameworkCore;
using Spoofer.Data;
using Spoofer.Services.Marker;
using Spoofer.Services.Navigation;
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
        private readonly IRegister _iRegister;
        private readonly IMarkerService _marker;
        private readonly ILogin _iLogin;
        public static CoordinatesContext _context;

        public App()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlServer(CONNECTION_STRING).Options;
            _context = new CoordinatesContext(options);
            _navigationStore = new NavigationStore();
            _iLogin = new ServiceLogin(_context, new NavigationService(_navigationStore, createMapViewModel));
            _iRegister = new ServiceRegister(_context);
            _marker = new MarkerService(_context, new NavigationService(_navigationStore, createSplashViewModel));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _navigationStore.BaseViewModel = createMapViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();
        }

        private MapViewModel createMapViewModel()
        {
            return new MapViewModel(_marker);
        }

        private AccountViewModel createAccountViewModel()
        {
            return new AccountViewModel(_iRegister, _iLogin);
        }
        public  SplashViewModel createSplashViewModel()
        {
            return new SplashViewModel( new NavigationService(_navigationStore, createMapViewModel));
        }
    }
}