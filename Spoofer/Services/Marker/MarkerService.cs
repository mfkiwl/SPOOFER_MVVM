using log4net;
using Spoofer.Commands.UserCommands;
using Spoofer.Data;
using Spoofer.Exceptions;
using Spoofer.Models;
using Spoofer.Services.Navigation;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Spoofer.Services.Marker
{
    public class MarkerService : IMarkerService
    {
        private int counter;
        private readonly CoordinatesContext _context;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly NavigationService _navigation;
        private string root = AppDomain.CurrentDomain.BaseDirectory;


        public MarkerService(CoordinatesContext context, NavigationService navigation)
        {
            _context = context;
            _navigation = navigation;
        }

        public void AddOrUpdateMarker(MapViewModel mapViewModel, bool isUpdated)
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
                    log.Debug($"{mapViewModel.Label} To Update");
                }
                var marker = new Coordinates()
                {
                    CoorfianteId = Guid.NewGuid().ToString(),
                    Latitude = mapViewModel.Latitude,
                    Longitude = mapViewModel.Longitude,
                    Height = mapViewModel.Height ?? 0,
                    Name = mapViewModel.Label.Trim(),
                    HasFile = BaseCommand.isFileExist(mapViewModel),
                    NumberInOrder = null
                };
                if (mapViewModel.SelectedItem != null)
                {

                    if (_context.Coordinates.Where(c => c.NumberInOrder == mapViewModel.SelectedItem && c.NumberInOrder > 0).Any())
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
                if (isUpdated)
                {

                    string fileName = $"{String.Concat(mapViewModel.Label.Where(c => !Char.IsWhiteSpace(c)))}.bin";
                    var file = new DirectoryInfo(root).GetFiles(fileName).SingleOrDefault(p => p.Exists);
                    if (file != null)
                    {
                        marker.GenerationDate = file.LastWriteTimeUtc.Date;
                    }
                    else
                    {
                        marker.GenerationDate = null;
                    }

                }

                _context.Add(marker);
                _context.SaveChanges();
                if (!isUpdated)
                {
                    MessageBox.Show($"{marker.Name} Added Succesfuly!!!!!");
                    log.Info(marker);
                }
                else
                {
                    MessageBox.Show($"{marker.Name} Updated Succesfully");
                    log.Info(marker);
                }
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
                string fileNameToDelete = $"{String.Concat(model.Label.Where(c => !Char.IsWhiteSpace(c)))}.bin";
                string[] realFileToDelete = Directory.GetFiles(root, fileNameToDelete);

                foreach (var file in realFileToDelete)
                {
                    if (!isUpdated)
                    {
                        File.Delete(file);
                        log.Debug($"{file} is deleted");
                    }
                }
                var coordinateToRemove = _context.Coordinates.SingleOrDefault(c => c.Name == model.Label);
                _context.Remove(coordinateToRemove);
                _context.SaveChanges(true);
                if (!isUpdated)
                {
                    MessageBox.Show($"{coordinateToRemove.Name.Trim()} Deleted Succesfully");
                    
                }
                log.Info($"{model.Label} Removed Succesfully");
            }
        }

        public bool isExist(MapViewModel mapViewModel)
        {
            if (_context.Coordinates.Any(p => p.Longitude == mapViewModel.Longitude && p.Longitude == mapViewModel.Longitude))
            {
                return true;
            }
            else if (_context.Coordinates.Any(p => p.Name == mapViewModel.Label))
            {
                throw new CoordinateExistException();
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

        public void Navigate()
        {
            _navigation.Navigate();
        }
        public void UpdateAfterDrop(Coordinates realcooSource, Coordinates realcootarget)
        {

            var tmpSource = realcooSource;
            var idSource = realcooSource.NumberInOrder;
            var tmpTarget = realcootarget;
            var idTarget = realcootarget.NumberInOrder;
            _context.Remove(realcooSource);
            _context.Remove(realcootarget);
            _context.SaveChanges();
            tmpSource.NumberInOrder = idTarget;
            tmpTarget.NumberInOrder = idSource;
            tmpSource.CoorfianteId = Guid.NewGuid().ToString();
            tmpTarget.CoorfianteId = Guid.NewGuid().ToString();
            _context.Coordinates.AddRange(tmpSource, tmpTarget);
            _context.SaveChanges();
            log.Info($"{realcooSource.Name} and {realcootarget.Name} switch their order");

        }

        public void RemoveFromList(Coordinates coordinate)
        {
            var tmp = coordinate;
            _context.Coordinates.Remove(coordinate);
            _context.SaveChanges();
            tmp.NumberInOrder = null;
            coordinate.CoorfianteId = Guid.NewGuid().ToString();
            _context.Coordinates.Add(coordinate);
            _context.SaveChanges();
            
            log.Info($"{coordinate.Name} is out of order");
        }
    }
}