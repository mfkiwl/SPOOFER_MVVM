using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Wpf.UI.Controls;
using Spoofer.Services.Marker;
using Spoofer.ViewModels;
using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Input;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using MapControl = Microsoft.Toolkit.Wpf.UI.Controls.MapControl;

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
        public virtual void onMapUpdated(MapControl mapControl, IMarkerService _service)
        {
            mapControl.MapElements.Clear();
            foreach (var location in _service.GetAll())
            {
                Windows.Devices.Geolocation.BasicGeoposition PinPosition = new Windows.Devices.Geolocation.BasicGeoposition
                {
                    Latitude = (double)location.Latitude,
                    Longitude = (double)location.Longitude,
                    Altitude = (double)location.Height
                };
                var geoPoint = new Windows.Devices.Geolocation.Geopoint(PinPosition);
                var mapIcon = new MapIcon()
                {
                    Location = geoPoint,
                    Image = RandomAccessStreamReference.CreateFromUri(new Uri("C:/Users/max/source/repos/Spoofer/Spoofer/Assets/icon.png")),
                    ZIndex = 0,
                    IsEnabled = true,
                    Title = location.Name
                };
                mapControl.MapElements.Add(mapIcon);
            }
        }
        public static bool isFileExist(MapViewModel mapViewModel)
        {
            var path = $@"C:\Users\max\source\repos\Spoofer\Spoofer\bin\Debug\{mapViewModel.Label}.bin";
            return File.Exists(path);
        }
        public static bool PingHost(string ipAddress)
        {
            var pinger = new Ping();
            var replay = pinger.Send(ipAddress);
            bool pingable = replay.Status == IPStatus.Success;
            return pingable;
        }
        public void OnException(Exception ex, ViewModelBase viewModel)
        {
            
        }
    }
}