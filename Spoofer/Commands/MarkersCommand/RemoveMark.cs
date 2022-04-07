using log4net;
using Microsoft.Toolkit.Wpf.UI.Controls;
using Spoofer.Commands.UserCommands;
using Spoofer.Services.Marker;
using Spoofer.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;

namespace Spoofer.Commands.MarkersCommand
{
    public class RemoveMark : BaseCommand
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMarkerService _service;
        private readonly MapViewModel _mapViewModel;

        public RemoveMark(MapViewModel mapViewModel, IMarkerService service)
        {
            _service = service;
            _mapViewModel = mapViewModel;
            _mapViewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChange();
        }

        public override bool CanExecute(object parameter)
        {
            var locationSelected = _service.GetAll().SingleOrDefault(l => l.Name == _mapViewModel.Label
            && l.Latitude == _mapViewModel.Latitude && l.Longitude == l.Longitude && l.Height == l.Height);
            return locationSelected != null && base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            try
            {
                _service.RemoveMarker(_mapViewModel);
                var map = parameter as MapControl;
                onMapUpdated(map, _service);
                log.Info("Makrer Removed Succesfully");
            }
            catch(Exception e)
            {
                log.Error("Cant Remove Marker Exception", e);
            }
        }
    }
}