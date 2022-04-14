using Spoofer.Models;
using Spoofer.ViewModels;
using System.Collections.Generic;
using Windows.Devices.Geolocation;

namespace Spoofer.Services.Marker
{
    public interface IMarkerService
    {
        void AddMarker(MapViewModel mapViewModel);

        void RemoveMarker(MapViewModel point);

        IEnumerable<Coordinates> GetAll();
        bool isExist(MapViewModel mapViewModel);

    }
}