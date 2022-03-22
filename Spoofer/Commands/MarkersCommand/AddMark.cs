using Spoofer.Commands.UserCommands;
using Spoofer.Services.Marker;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoofer.Commands.MarkersCommand
{
    public class AddMark : BaseCommand
    {
        private readonly MapViewModel _mapViewModel;
        private readonly IMarkerService _service;
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
        }
    }
}
