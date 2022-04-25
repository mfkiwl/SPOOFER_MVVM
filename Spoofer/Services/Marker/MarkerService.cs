using log4net;
using Spoofer.Data;
using Spoofer.Models;
using Spoofer.Services.Navigation;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

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
                if (String.IsNullOrEmpty(mapViewModel.Label))
                {
                    mapViewModel.ErrorMessageViewModel.ErrorMessage = "No Label Defined For this marker";


                }
                else if (mapViewModel.Longitude > 180 || mapViewModel.Longitude < -180 ||
                         mapViewModel.Latitude > 90 || mapViewModel.Latitude < -90)
                {
                    mapViewModel.ErrorMessageViewModel.ErrorMessage = "Coordiantes values are invalid \n" +
                        "Latitude: Between -90 to 90, Longitude: Between -180 to 180.";


                }
                else if (isExist(mapViewModel))
                {
                    mapViewModel.ErrorMessageViewModel.ErrorMessage = "There is marker on this location already...";
                    
                }
                else
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
                    MessageBox.Show($"{marker.Name} Added Succesfuly!!!!!");
                }
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
                model.ErrorMessageViewModel.Refresh();
                if (!isExist(model))
                {
                    model.ErrorMessageViewModel.ErrorMessage = "No Marker To Delete";
                }
                else if (String.IsNullOrEmpty(model.Label))
                {
                    model.ErrorMessageViewModel.ErrorMessage = "Label not specefied for the marker you want to delete";
                }
                else
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
            }
            catch (Exception ex)
            {
                log.Error("Invalid Operation Excaption!!!!!!!!!!", ex);
                MessageBox.Show(ex.Message);

                return;
            }
        }

        public bool isExist(MapViewModel mapViewModel)
        {
            if (_context.Coordinates.Any(p=>p.Name == mapViewModel.Label || p.Longitude == mapViewModel.Longitude && p.Longitude == mapViewModel.Longitude))
            {
                return true;
            }
            return false;
        }
    }
}