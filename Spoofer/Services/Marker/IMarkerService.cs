﻿using Spoofer.Models;
using Spoofer.ViewModels;
using System.Collections.Generic;
using Windows.Devices.Geolocation;

namespace Spoofer.Services.Marker
{
    public interface IMarkerService
    {
        void AddMarker(MapViewModel mapViewModel);

        void RemoveMarker(Geopoint point);

        IEnumerable<Coordinates> GetAll();

        Coordinates GetCoordinateByLocation(Geopoint point);
    }
}