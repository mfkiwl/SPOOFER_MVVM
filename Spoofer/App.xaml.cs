using Spoofer.Services.Navigation;
using Spoofer.Services;
using Spoofer.Services.User;
using Spoofer.Stores;
using Spoofer.ViewModels;
using Spoofer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Spoofer.Services.Marker;

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
            _marker = new MarkerService(_context);
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
    }
}
