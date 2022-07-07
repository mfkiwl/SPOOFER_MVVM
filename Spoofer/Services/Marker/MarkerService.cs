using log4net;
using Spoofer.Commands.UserCommands;
using Spoofer.Data;
using Spoofer.Exceptions;
using Spoofer.Models;
using Spoofer.Services.Navigation;
using Spoofer.Services.User.Repository;
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
        private readonly IRepository<Coordinates> _coordinateRepo;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly NavigationService _navigation;
        private string root = AppDomain.CurrentDomain.BaseDirectory;


        public MarkerService(IRepository<Coordinates> coordinateRepo, NavigationService navigation)
        {
            _coordinateRepo = coordinateRepo;
            _navigation = navigation;
        }
        /// <summary>
        /// Add or update specific location by User specification on the map.
        /// </summary>
        /// <param name="mapViewModel">
        /// Location
        /// </param>
        /// <param name="isUpdated">
        /// True When User Is Exist and need to be Updated, False when the user add it for the first time.
        /// </param>
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
                    Id = Guid.NewGuid().ToString(),
                    Latitude = mapViewModel.Latitude,
                    Longitude = mapViewModel.Longitude,
                    Height = mapViewModel.Height ?? 0,
                    Name = mapViewModel.Label.Trim(),
                    HasFile = BaseCommand.isFileExist(mapViewModel),
                    NumberInOrder = null
                };
                if (mapViewModel.SelectedItem != null)
                {

                    if (_coordinateRepo.GetAll().Where(c => c.NumberInOrder == mapViewModel.SelectedItem && c.NumberInOrder > 0).Any())
                    {
                        throw new InvalidCoordinateException("There is Marker on This Place In order to transmition, please edit the list..");
                    }
                    marker.NumberInOrder = mapViewModel.SelectedItem;
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

                _coordinateRepo.AddOrUpdate(marker);
                _coordinateRepo.Save();
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
        /// <summary>
        /// Get All The Coordinates from the Database.
        /// </summary>
        /// <returns>
        /// List Of Coordinates Entities 
        /// </returns>
        public IEnumerable<Coordinates> GetAll()
        {
            return _coordinateRepo.GetAll();
        }
        /// <summary>
        /// Remove Marker From The Database and the Map.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isUpdated"></param>
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
                string fileNameToDelete = $"{String.Concat(model.Label.Where(c => !char.IsWhiteSpace(c)))}.bin";
                string[] realFileToDelete = Directory.GetFiles(root, fileNameToDelete);

                foreach (var file in realFileToDelete)
                {
                    if (!isUpdated)
                    {
                        File.Delete(file);
                        log.Debug($"{file} is deleted");
                    }
                }
                var coordinateToRemove = _coordinateRepo.GetAll().SingleOrDefault(c => c.Name.Trim() == model.Label.Trim());
                _coordinateRepo.Remove(coordinateToRemove.Id);
                _coordinateRepo.Save();
                if (!isUpdated)
                {
                    MessageBox.Show($"{coordinateToRemove.Name.Trim()} Deleted Succesfully");
                    
                }
                log.Info($"{model.Label} Removed Succesfully");
            }
        }
        /// <summary>
        /// Check If The Marker User chose exists on the Database
        /// </summary>
        /// <param name="mapViewModel"></param>
        /// <returns>
        /// Returns True when The Marker specefied on the database and false if he did not specefied.
        /// </returns>
        public bool isExist(MapViewModel mapViewModel)
        {
            if (_coordinateRepo.GetAll().Any(p => p.Longitude == mapViewModel.Longitude && p.Longitude == mapViewModel.Longitude))
            {
                return true;
            }
            else if (_coordinateRepo.GetAll().Any(p => p.Name == mapViewModel.Label))
            {
                throw new CoordinateExistException();
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapViewModel"></param>
        /// <returns></returns>
        public Coordinates GetCoordinateByViewModel(MapViewModel mapViewModel)
        {
            var coordinate = _coordinateRepo.GetAll().SingleOrDefault(c => c.Name == mapViewModel.Label &&
                                                                     c.Longitude == mapViewModel.Longitude &&
                                                                     c.Latitude == mapViewModel.Latitude);
            return coordinate;
        }
        /// <summary>
        /// Navigate Between Pages.
        /// </summary>
        public void Navigate()
        {
            _navigation.Navigate();
        }
        /// <summary>
        /// Changing The Order Of Coordinates In The Sequence Table By Drag and drop
        /// </summary>
        /// <param name="realcooSource"></param>
        /// <param name="realcootarget"></param>
        public void UpdateAfterDrop(Coordinates realcooSource, Coordinates realcootarget)
        {

            var tmpSource = realcooSource;
            var idSource = realcooSource.NumberInOrder;
            var tmpTarget = realcootarget;
            var idTarget = realcootarget.NumberInOrder;
            tmpSource.NumberInOrder = idTarget;
            tmpTarget.NumberInOrder = idSource;
            _coordinateRepo.Update(tmpSource, tmpTarget);
            _coordinateRepo.Save();
            log.Info($"{realcooSource.Name} and {realcootarget.Name} switch their order");

        }
        /// <summary>
        /// Remove From The List In Order To Transmit
        /// </summary>
        /// <param name="coordinate"></param>
        public void RemoveFromList(Coordinates coordinate)
        {

            var tmp = coordinate;
            tmp.NumberInOrder = null;
            _coordinateRepo.Update(tmp);
            _coordinateRepo.Save();
            log.Info($"{coordinate.Name} is out of order");
        }
    }
}