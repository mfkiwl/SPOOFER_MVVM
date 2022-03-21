using log4net;
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
                    Name = mapViewModel.Label
                };
                foreach (var user in _context.User)
                {
                    if ((bool)user.IsAuthenticated)
                    {
                        marker.UserId = user.UserId;
                        _context.Add(marker);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Invalid Operation Excaption!!!!!!!!!!", ex);
                return;
            }
        }

        public List<Coordinates> GetAll()
        {
            return _context.Coordinates.ToList();
        }

        public Coordinates GetCoordinateById(string id)
        {
            try
            {
                var coordinate = _context.Coordinates.SingleOrDefault(p => p.CoorfianteId == id);
                return coordinate;
            }
            catch (Exception ex)
            {
                log.Error("Invalid Operation Excaption!!!!!!!!!!", ex);
                return null;
            }
        }

        public void RemoveMarker(string id)
        {
            try
            {
                var coordinateToRemove = _context.Coordinates.SingleOrDefault(p => p.CoorfianteId == id);
                _context.Remove(coordinateToRemove);
                _context.SaveChanges(true);
            }
            catch (Exception ex)
            {
                log.Error("Invalid Operation Excaption!!!!!!!!!!", ex);
                return;
            }

        }
    }
}
