using Spoofer.Commands.UserCommands;
using Spoofer.Services.Marker;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;

namespace Spoofer.Commands.MarkersCommand
{
    public class GetMark : BaseCommand
    {
        private readonly IMarkerService _service;
        public GetMark(IMarkerService service)
        {
            _service = service;
        }
        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }
        public override void Execute(object parameter)
        {
            _service.GetCoordinateByLocation((Geopoint)parameter);
        }
    }
}
