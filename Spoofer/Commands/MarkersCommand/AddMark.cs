using Spoofer.Commands.UserCommands;
using Spoofer.EXMethods;
using Spoofer.Services.Marker;
using Spoofer.ViewModels;
using System;
using System.Windows;
using System.Windows.Threading;
using Windows.UI.Core;

namespace Spoofer.Commands.MarkersCommand
{
    public class AddMark : BaseCommand
    {
        DispatcherTimer dt = new DispatcherTimer();
        private readonly MapViewModel _mapViewModel;
        private readonly IMarkerService _service;
        private int argc;

        public AddMark(MapViewModel mapViewModel, IMarkerService service)
        {
            _mapViewModel = mapViewModel;
            _service = service;
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
            //return base.CanExecute(parameter) && !double.IsNaN(_mapViewModel.Longitude) &&
            //!double.IsNaN(_mapViewModel.Longitude) &&
            //!string.IsNullOrEmpty(_mapViewModel.Label);
        }

        public override void Execute(object parameter)
        {
            dt.IsEnabled = true;
            dt.Interval = TimeSpan.FromSeconds(125);
            dt.Tick += Dt_Tick;
            
            //dt.Stop();

        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            //var splashScreen = new SplashScreen("C:/Users/max/source/repos/Spoofer/Spoofer/Assets/icon.png");
            //splashScreen.Show(true);
            argc = GenerateFlags().Length;
            var argv = GenerateFlags();
            SpoofingMethods.main(argc, argv)/*))*/;
            _service.AddMarker(_mapViewModel);
            /*Application.Current.Dispatcher.Invoke((Action)(() => */
            dt.IsEnabled = false;

        }

        private string[] GenerateFlags()
        {
            var flags = new string[9];
            flags[0] = "Spoofer.exe";
            flags[1] = "-e";
            flags[2] = "brdc3540.14n";
            flags[3] = "-s";
            flags[4] = "2600000";
            flags[5] = "-l";
            flags[6] = $"{_mapViewModel.Latitude},{_mapViewModel.Longitude},{_mapViewModel.Height}";
            flags[7] = "-o";
            flags[8] = $"{_mapViewModel.Label}.bin";
            return flags;
        }
    }
}