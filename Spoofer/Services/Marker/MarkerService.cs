using log4net;
using Spoofer.Commands.UserCommands;
using Spoofer.Data;
using Spoofer.Exceptions;
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
        private readonly CoordinatesContext _context;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MarkerService(CoordinatesContext context)
        {

            _context = context;
        }

        public void AddOrUpdateMarker(MapViewModel mapViewModel)
        {

            if (String.IsNullOrEmpty(mapViewModel.Label))
            {
                throw new InvalidCoordinateException("No Label Defined For this marker");
            }
            else if (mapViewModel.Longitude > 180 || mapViewModel.Longitude < -180 ||
                     mapViewModel.Latitude > 90 || mapViewModel.Latitude < -90)
            {
                throw new InvalidCoordinateException("Coordiantes values are invalid \n" +
                                                      "Latitude: Between -90 to 90, Longitude: Between -180 to 180.");
            }
            else
            {

                if (isExist(mapViewModel))
                {
                    RemoveMarker(mapViewModel, true);
                }
                var marker = new Coordinates()
                {
                    CoorfianteId = Guid.NewGuid().ToString(),
                    Latitude = mapViewModel.Latitude,
                    Longitude = mapViewModel.Longitude,
                    Height = mapViewModel.Height ?? 0,
                    Name = mapViewModel.Label ?? "",
                    HasFile = BaseCommand.isFileExist(mapViewModel),
                    NumberInOrder = null
                };
                if (mapViewModel.SelectedItem != null)
                {
                    if (_context.Coordinates.Where(c => c.NumberInOrder == mapViewModel.SelectedItem).Any())
                    {
                        throw new InvalidCoordinateException("There is Marker on This Place In order to transmition, please edit the list..");
                    }
                    marker.NumberInOrder = mapViewModel.SelectedItem;
                }
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

        public IEnumerable<Coordinates> GetAll()
        {
            return _context.Coordinates.ToList();
        }

        public void RemoveMarker(MapViewModel model, bool isUpdated)
        {

            model.ErrorMessageViewModel.Refresh();
            if (!isExist(model))
            {
                throw new CoordinateNotExistException();
            }
            else if (String.IsNullOrEmpty(model.Label))
            {
                throw new InvalidCoordinateException("Label not specefied for the marker you want to delete");
            }
            else
            {
                string root = @"C:\Users\max\source\repos\Spoofer\Spoofer\bin\Debug";
                string fileNameToDelete = $"{String.Concat(model.Label.Where(c => !Char.IsWhiteSpace(c)))}.bin";
                string[] realFileToDelete = System.IO.Directory.GetFiles(root, fileNameToDelete);

                foreach (var file in realFileToDelete)
                {
                    if (!isUpdated)
                    {
                        System.IO.File.Delete(file);
                    }
                }
                var coordinateToRemove = _context.Coordinates.SingleOrDefault(c => c.Name == model.Label);
                _context.Remove(coordinateToRemove);
                _context.SaveChanges(true);
                if (!isUpdated)
                {
                    MessageBox.Show($"{coordinateToRemove.Name} Deleted Succesfully");
                }
            }
        }

        public bool isExist(MapViewModel mapViewModel)
        {
            if (_context.Coordinates.Any(p => p.Name == mapViewModel.Label || p.Longitude == mapViewModel.Longitude && p.Longitude == mapViewModel.Longitude))
            {
                return true;
            }
            return false;
        }

        public Coordinates GetCoordinateByViewModel(MapViewModel mapViewModel)
        {
            var coordinate = _context.Coordinates.SingleOrDefault(c => c.Name == mapViewModel.Label &&
                                                                     c.Longitude == mapViewModel.Longitude &&
                                                                     c.Latitude == mapViewModel.Latitude);
            return coordinate;
        }


    }
}