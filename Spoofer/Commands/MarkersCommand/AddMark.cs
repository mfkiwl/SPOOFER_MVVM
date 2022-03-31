using Spoofer.Commands.UserCommands;
using Spoofer.EXMethods;
using Spoofer.Services.Marker;
using Spoofer.ViewModels;

namespace Spoofer.Commands.MarkersCommand
{
    public class AddMark : BaseCommand
    {
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
            _service.AddMarker(_mapViewModel);
            argc = GenerateFlags().Length;
            SpoofingMethods.main(argc, GenerateFlags());
        }
        private string[] GenerateFlags()
        {
            var flags = new string[7];
            flags[0] = "Spoofer.exe";
            flags[1] = "-e";
            flags[2] = "brdc3540.14n";
            flags[3] = "-s";
            flags[4] = "2600000";
            flags[5] = "-l";
            flags[6] = $"{_mapViewModel.Latitude},{_mapViewModel.Longitude},{_mapViewModel.Height}";
            //flags[7] = $"{_mapViewModel.Longitude}";
            //flags[8] = $"{_mapViewModel.Height}";
            return flags;
        }
    }
}