using log4net;
using Microsoft.Toolkit.Wpf.UI.Controls;
using Spoofer.Commands.UserCommands;
using Spoofer.EXMethods;
using Spoofer.Services.Marker;
using Spoofer.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls.Maps;
using MapControl = Microsoft.Toolkit.Wpf.UI.Controls.MapControl;

namespace Spoofer.Commands.MarkersCommand
{
    public class AddMark : BaseCommand
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly MapViewModel _mapViewModel;
        private readonly IMarkerService _service;
        

        public AddMark(MapViewModel mapViewModel, IMarkerService service)
        {
            _mapViewModel = mapViewModel;
            _service = service;
            _mapViewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_mapViewModel.Latitude) &&
                e.PropertyName == nameof(_mapViewModel.Longitude) && e.PropertyName == nameof(_mapViewModel.Label))
            {
                OnCanExecuteChange();
            }
        }

        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter) &&
                _mapViewModel.Longitude <= 90 &&
                _mapViewModel.Longitude >= -90 &&
                _mapViewModel.Latitude <= 180 &&
                _mapViewModel.Latitude >= -180;
        }

        public override void Execute(object parameter)
        {
            try
            {
                _service.AddMarker(_mapViewModel);
                var map = parameter as MapControl;
                onMapUpdated(map, _service);
                log.Info($"New Marker {_mapViewModel.Label} Was Added");
            }
            catch (Exception ex)
            {
                log.Error("Invalid Operation Exception!!!!!!!!!!!!!!!!!", ex);
                return;
            }
        }
        public override void onMapUpdated(MapControl mapControl, IMarkerService _service)
        {
            base.onMapUpdated(mapControl, _service);
        }

    }
}