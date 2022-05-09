using Spoofer.Models;
using Spoofer.ViewModels;
using System.Collections.Generic;
using Windows.Devices.Geolocation;

namespace Spoofer.Services.Marker
{
    public interface IMarkerService
    {
        void AddOrUpdateMarker(MapViewModel mapViewModel);

        void RemoveMarker(MapViewModel mapViewModel);

        IEnumerable<Coordinates> GetAll();
        bool isExist(MapViewModel mapViewModel);
        Coordinates GetCoordinateByViewModel(MapViewModel mapViewModel);
        

    }
}