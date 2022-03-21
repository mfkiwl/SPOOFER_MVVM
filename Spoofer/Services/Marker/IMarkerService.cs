using Spoofer.Models;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoofer.Services.Marker
{
    public interface IMarkerService
    {
        void AddMarker(MapViewModel mapViewModel);
        void RemoveMarker(string id);
        List<Coordinates> GetAll();
        Coordinates GetCoordinateById(string id);
    }
}
