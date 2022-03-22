using log4net;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Spoofer.Data;
using Spoofer.Models;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spoofer.Services.Marker
{
    public class MarkerService : IMarkerService
    {
        private readonly CoordinatesContext _context;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MarkerService(CoordinatesContext context)
        {
            _context = context;
        }
        public void AddMarker(MapViewModel mapViewModel)
        {
            try
            {
                var marker = new Coordinates()
                {
                    CoorfianteId = Guid.NewGuid().ToString(),
                    Latitude = mapViewModel.Latitude,
                    Longitude = mapViewModel.Longitude,
                    Height = mapViewModel.Height ?? 0,
                    Name = mapViewModel.Label ?? "",

                };
                foreach (var user in _context.User)
                {
                    user.IsAuthenticated = true;
                    if ((bool)user.IsAuthenticated)
                    {
                        marker.UserId = user.UserId;
                    }
                }
                _context.Add(marker);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                log.Error("Invalid Operation Excaption!!!!!!!!!!", ex);
                return;
            }
        }

        public IEnumerable<Coordinates> GetAll()
        {
            return _context.Coordinates.ToList();
        }

        public void RemoveMarker(Windows.Devices.Geolocation.Geopoint point)
        {
            try
            {
                var coordinateToRemove = GetCoordinateByLocation(point);
                _context.Remove(coordinateToRemove);
                _context.SaveChanges(true);
            }
            catch (Exception ex)
            {
                log.Error("Invalid Operation Excaption!!!!!!!!!!", ex);
                return;
            }
        }

        public Coordinates GetCoordinateByLocation(Windows.Devices.Geolocation.Geopoint point)
        {
            try
            {
                var coordinate = _context.Coordinates.SingleOrDefault(p => p.Latitude == point.Position.Latitude && p.Longitude == point.Position.Longitude && p.Height == point.Position.Altitude);
                return coordinate;
            }
            catch (Exception ex)
            {
                log.Error("Invalid Operation Excaption!!!!!!!!!!", ex);
                return null;
            }
        }
    }
}
