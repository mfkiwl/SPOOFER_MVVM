using log4net;
using Microsoft.EntityFrameworkCore;
using Spoofer.Data;
using Spoofer.Models;
using Spoofer.Services.Marker;
using Spoofer.Services.Navigation;
using Spoofer.Services.Spoofer;
using Spoofer.Services.User;
using Spoofer.Services.User.Register;
using Spoofer.Services.User.Repository;
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
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //"Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True; = Release
        private const string CONNECTION_STRING = "Server=(localdb)\\mssqllocaldb;Database=Coordinates;Trusted_Connection=True;";
        private readonly NavigationStore _navigationStore;
        public static CoordinatesContext _context;
        private readonly IMarkerService _marker;
        private readonly IMarkerService _tableMarker;
        private readonly IMarkerService _manageMarker;
        private readonly ILogin _iLogin;
        private readonly IRegister _register;
        private readonly ISpooferService _spoofer;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Coordinates> _coordinateRepo;


        public App()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlServer(CONNECTION_STRING).Options;
            _context = new CoordinatesContext(options);
            _userRepo = new MyRepository<User>(_context);
            _coordinateRepo = new MyRepository<Coordinates>(_context);
            _navigationStore = new NavigationStore();
            _marker = new MarkerService(_coordinateRepo, new NavigationService(_navigationStore, createTransmitViewModel));
            _tableMarker = new MarkerService(_coordinateRepo, new NavigationService(_navigationStore, createMapViewModel));
            _manageMarker = new MarkerService(_coordinateRepo, new NavigationService(_navigationStore, createUserViewModel));
            _spoofer = new SpooferService(_marker);
            _iLogin = new ServiceLogin(_userRepo, _coordinateRepo, new NavigationService(_navigationStore, createMapViewModel), _spoofer);
            _register = new ServiceRegister(_context);
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _context.Database.EnsureCreated();
            _navigationStore.BaseViewModel = createAccountViewModel();
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore),
            };

            MainWindow.Show();
            MainWindow.Closed += (sender, args) =>
            {
                foreach (var entity in _userRepo.GetAll())
                {
                    var user = new User();
                    user = entity;
                    user.IsAuthenticated = false;
                    _userRepo.Update(user);
                }
                _userRepo.Save();
            };
        }



        private MapViewModel createMapViewModel()
        {
            return new MapViewModel(_marker, _spoofer);
        }

        private LoginViewModel createAccountViewModel()
        {
            return new LoginViewModel(_iLogin, _register);
        }
        private TransmitInOrderViewModel createTransmitViewModel()
        {
            var vm = new TransmitInOrderViewModel(_tableMarker,_manageMarker, _spoofer, _userRepo);
            _spoofer.GenerateInOrder(vm);
            return vm;
        }
        private UserViewModel createUserViewModel()
        {
            return new UserViewModel(_userRepo);
        }
        

    }
}