using log4net;
using Spoofer.Data;
using Spoofer.Models;
using Spoofer.Services.Navigation;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Spoofer.Services.Marker
{
    public class MarkerService : IMarkerService
    {
        private int counter;
        private readonly NavigationService _navigation;
        private readonly CoordinatesContext _context;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MarkerService(CoordinatesContext context, NavigationService navigation)
        {
            _context = context;
            _navigation = navigation;
        }

        public void AddMarker(MapViewModel mapViewModel)
        {
            try
            {
                counter++;
                if (counter == 1)
                {
                    _navigation.Navigate();
                }
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
                MessageBox.Show($"{marker.Name} Added Succesfuly!!!!!");
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

        public void RemoveMarker(MapViewModel model)
        {
            try
            {
                string root = @"C:\Users\max\source\repos\Spoofer\Spoofer\bin\Debug";
                string fileNameToDelete = $"{model.Label}.bin";
                string[] realFileToDelete = System.IO.Directory.GetFiles(root, fileNameToDelete);
                foreach (var file in realFileToDelete)
                {
                    System.IO.File.Delete(file);
                }
                var coordinateToRemove = _context.Coordinates.SingleOrDefault(c => c.Name == model.Label);
                _context.Remove(coordinateToRemove);
                _context.SaveChanges(true);
                MessageBox.Show($"{coordinateToRemove.Name} Deleted Succesfully");
            }
            catch (Exception ex)
            {
                log.Error("Invalid Operation Excaption!!!!!!!!!!", ex);
                return;
            }
        }

      

    }
}