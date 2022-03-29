﻿using Spoofer.EXMethods;
using Spoofer.Services.Marker;
using Spoofer.Structs;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using MapControl = Microsoft.Toolkit.Wpf.UI.Controls.MapControl;
using UserControl = System.Windows.Controls.UserControl;

namespace Spoofer.Views
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        private readonly IMarkerService _markerService;
        private bool isIconSigned = false;
        double[] llh = new double[3];
        Ephem_T[,] eph = new Ephem_T[13, 33];
        GPSTime_T g0;
        Channel_T[] chan = new Channel_T[16];
        GPSTime_T grx, gmin, gmax, gtmp;
        DateTime_T t0, tmin, tmax;
        IonOutC_T ionoutc;
        DateTime_T ttmp;
        Range_T rho;
        public MapView()
        {
            InitializeComponent();
            _markerService = new MarkerService(App._context);
            
        }

        private async  void MapControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (var location in _markerService.GetAll())
            {
                BasicGeoposition PinPosition = new BasicGeoposition
                {
                    Latitude = (double)location.Latitude,
                    Longitude = (double)location.Longitude,
                    Altitude = (double)location.Height
                };
                int number = EXMethods.SpoofingMethods.MyMethod();
                var geoPoint = new Geopoint(PinPosition);
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
            if (mapControl.MapElements.Any())
            {
                var lastLocationAdded = _markerService.GetAll().FirstOrDefault(p => p.Latitude >= 20);
                BasicGeoposition lastPos = new BasicGeoposition()
                {
                    Latitude = (double)lastLocationAdded.Latitude,
                    Longitude = (double)lastLocationAdded.Longitude
                };
                var lastPosPoint = new Geopoint(lastPos);
                await (sender as MapControl).TrySetViewAsync(lastPosPoint, 17);
            }
        }

        private void MapControl_MapElementClick(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.MapElementClickEventArgs e)
        {
            CancelTemporaryIcons();
            foreach (var element in e.MapElements)
            {
                var signedElement = element as MapIcon;
                if (!String.IsNullOrEmpty(signedElement.Title))
                {
                    isIconSigned = true;
                    lat.Text = signedElement.Location.Position.Latitude.ToString();
                    lon.Text = signedElement.Location.Position.Longitude.ToString();
                    alt.Text = signedElement.Location.Position.Altitude.ToString();
                    lab.Text = signedElement.Title;
                    double user = signedElement.Location.Position.Latitude;
                }
                else
                {
                    mapControl.MapElements.Remove(element);
                    isIconSigned = false;
                }
            }
        }
        private void mapControl_MapDoubleTapped(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.MapInputEventArgs e)
        {
            CancelTemporaryIcons();
            var mousePoint = e.Location;
            lat.Text = mousePoint.Position.Latitude.ToString();
            lon.Text = mousePoint.Position.Longitude.ToString();
            var mapIcon = new MapIcon()
            {
                Location = mousePoint,
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("C:/Users/max/source/repos/Spoofer/Spoofer/Assets/icon.png")),
                ZIndex = 0,
                IsEnabled = false
            };
            mapControl.MapElements.Add(mapIcon);
            SpoofingMethods.main(llh, eph, g0, chan, grx, t0, tmin, tmax, gmin, gmax, ionoutc, gtmp, ttmp, rho);
        }

        private void CancelTemporaryIcons()
        {
            if (mapControl.MapElements.Count() > _markerService.GetAll().Count())
            {
                mapControl.MapElements.RemoveAt(mapControl.MapElements.Count() - 1);
            }
        }
    }
}