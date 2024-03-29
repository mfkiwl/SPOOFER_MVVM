﻿using Spoofer.Services.Marker;
using Spoofer.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Input;
using Windows.UI.Xaml.Controls.Maps;
using MapControl = Microsoft.Toolkit.Wpf.UI.Controls.MapControl;
using GeoPoint = Windows.Devices.Geolocation.Geopoint;
using BasicGeoPosition = Windows.Devices.Geolocation.BasicGeoposition;

namespace Spoofer.Commands.UserCommands
{
    public abstract class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }
        public abstract void Execute(object parameter);

        public void OnCanExecuteChange()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
        public virtual void OnMapUpdated(MapControl mapControl, IMarkerService service)
        {
            mapControl.MapElements.Clear();
            foreach (var location in service.GetAll())
            {
                BasicGeoPosition PinPosition = new BasicGeoPosition
                {
                    Latitude = (double)location.Latitude,
                    Longitude = (double)location.Longitude,
                    Altitude = (double)location.Height
                };
                var geoPoint = new GeoPoint(PinPosition);
                var mapIcon = new MapIcon()
                {
                    Location = geoPoint,
                    ZIndex = 0,
                    IsEnabled = true,
                    Title = location.Name
                };
                mapControl.MapElements.Add(mapIcon);
            }
        }
        public static bool isFileExist(MapViewModel mapViewModel)
        {
            var path = $@"{AppDomain.CurrentDomain.BaseDirectory}/{String.Concat(mapViewModel.Label.Where(c => !Char.IsWhiteSpace(c)))}.bin";
            return File.Exists(path);
        }
        public static bool PingHost(string ipAddress)
        {
            var pinger = new Ping();
            var replay = pinger.Send(ipAddress, 100);
            bool pingable = replay.Status == IPStatus.Success;
            return pingable;
        }


    }
}