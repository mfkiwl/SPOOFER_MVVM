using Spoofer.EXMethods;
using Spoofer.Services.Marker;
using Spoofer.Services.Navigation;
using Spoofer.Stores;
using Spoofer.ViewModels;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using MapControl = Microsoft.Toolkit.Wpf.UI.Controls.MapControl;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace Spoofer.Views
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        private readonly NavigationService _navigationService;
        private readonly IMarkerService _markerService;
        private bool isIconSigned;
        private void mapControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            e.Handled = true;
            var capturedMouse = e.GetPosition(this);
            var position = mapControl.TranslatePoint(capturedMouse, mapControl);
            lat.Text = position.X.ToString();
            lon.Text = position.Y.ToString();
        }

        public MapView()
        {
            InitializeComponent();
            _navigationService = new NavigationService();
            _markerService = new MarkerService(App._context, _navigationService);

        }

        private async void MapControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (var location in _markerService.GetAll())
            {
                BasicGeoposition PinPosition = new BasicGeoposition
                {
                    Latitude = (double)location.Latitude,
                    Longitude = (double)location.Longitude,
                    Altitude = (double)location.Height
                };
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
            };
            if (mapControl.MapElements.Any())
            {
                var lastLocationAdded = _markerService.GetAll().Last();
                BasicGeoposition lastPos = new BasicGeoposition()
                {
                    Latitude = (double)lastLocationAdded.Latitude,
                    Longitude = (double)lastLocationAdded.Longitude
                };
                var lastPosPoint = new Geopoint(lastPos);
                await (sender as MapControl).TrySetViewAsync(lastPosPoint, 13);
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