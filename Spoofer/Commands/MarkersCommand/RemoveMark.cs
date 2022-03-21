using Spoofer.Commands.UserCommands;
using Spoofer.Services.Marker;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoofer.Commands.MarkersCommand
{
    public class RemoveMark : BaseCommand
    {
        private readonly IMarkerService _service;
        private readonly MapViewModel _mapViewModel;
        public RemoveMark(MapViewModel mapViewModel, IMarkerService service)
        {
            _service = service;
            _mapViewModel = mapViewModel;
        }
        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter) && _mapViewModel.SelectedMarkerId != null;
        }
        public override void Execute(object parameter)
        {
            _service.RemoveMarker(_mapViewModel.SelectedMarkerId);
        }
    }
}
