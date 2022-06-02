using Spoofer.Commands.UserCommands;
using Spoofer.Services.Marker;
using Spoofer.Services.Navigation;
using Spoofer.ViewModels;
using System;
using System.Device.Location;
using System.Linq;
using System.Windows;
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
        private readonly NavigationService _navigationService;
        private readonly IMarkerService _markerService;
        private bool isIconSigned;


        public MapView()
        {
            InitializeComponent();
            _navigationService = new NavigationService();
            _markerService = new MarkerService(App._context, _navigationService);


        }
        private void mapControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

            e.Handled = true;
            var capturedMouse = e.GetPosition(this);
            var position = mapControl.TranslatePoint(capturedMouse, mapControl);
            lat.Text = position.X.ToString();
            lon.Text = position.Y.ToString();
        }

        private async void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.Start();
            GeoCoordinate coord = watcher.Position.Location;
            if (!watcher.Position.Location.IsUnknown)
            {
                BasicGeoposition currentPoint = new BasicGeoposition
                {
                    Latitude = coord.Latitude,
                    Longitude = coord.Longitude
                };
                var currentGeoPoint = new Geopoint(currentPoint);
                var currentMapIcon = new MapIcon()
                {
                    Location = currentGeoPoint,
                    //Image = RandomAccessStreamReference.CreateFromUri(new Uri("~/Assets/icon.png")),
                    ZIndex = 0,
                    IsEnabled = true,
                    Title = "Your Current Location",
                };
                mapControl.MapElements.Add(currentMapIcon);
                await (sender as MapControl).TrySetViewAsync(currentGeoPoint, 13);
            }
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
                    //Image = RandomAccessStreamReference.CreateFromUri(new Uri("~/Assets/icon.png")),
                    ZIndex = 0,
                    IsEnabled = true,
                    Title = location.Name,
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

        private async  void MapControl_MapElementClick(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.MapElementClickEventArgs e)
        {

            var vm = (MapViewModel)DataContext;
            if (vm.IsFinishLoading)
            {
                CancelTemporaryIcons();
                DeleteTextboxes();
                _border.Visibility = Visibility.Collapsed;
                foreach (var element in e.MapElements)
                {
                    var signedElement = element as MapIcon;

                    if (!String.IsNullOrEmpty(signedElement.Title))
                    {
                        await ((MapControl)sender).TrySetViewAsync(signedElement.Location, 16);
                        isIconSigned = true;
                        lat.Text = signedElement.Location.Position.Latitude.ToString();
                        lon.Text = signedElement.Location.Position.Longitude.ToString();
                        alt.Text = signedElement.Location.Position.Altitude.ToString();
                        lab.Text = signedElement.Title.Trim();
                        double user = signedElement.Location.Position.Latitude;
                        var realMarker = _markerService.GetAll().SingleOrDefault(p => p.Name == signedElement.Title &&
                        (double)p.Height == signedElement.Location.Position.Altitude &&
                        p.Longitude == signedElement.Location.Position.Longitude &&
                        p.Latitude == signedElement.Location.Position.Latitude);
                        Combo.SelectedItem = realMarker.NumberInOrder;
                        vm.IsFileCreated = BaseCommand.isFileExist(vm);
                        if (realMarker.NumberInOrder < 1)
                        {
                            Combo.SelectedItem = Combo.Text = "";
                        }
                        if (realMarker != null)
                        {
                            _border.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        mapControl.MapElements.Remove(element);
                        isIconSigned = false;
                    }
                }
            }
        }
        private void mapControl_MapDoubleTapped(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.MapInputEventArgs e)
        {

            var vm = (MapViewModel)DataContext;
            if (!vm.IsLoading)
            {
                CancelTemporaryIcons();
                DeleteTextboxes();
                Combo.SelectedItem = Combo.Text;
                _border.Visibility = Visibility.Collapsed;
                var mousePoint = e.Location;
                lat.Text = mousePoint.Position.Latitude.ToString();
                lon.Text = mousePoint.Position.Longitude.ToString();
                var mapIcon = new MapIcon()
                {
                    Location = mousePoint,
                    Image = RandomAccessStreamReference.CreateFromUri(new Uri(@"C:/Users/max/source/repos/Spoofer/Spoofer/Assets/icon.png")),
                    ZIndex = 0,
                    IsEnabled = false
                };
                mapControl.MapElements.Add(mapIcon);
            }
        }

        private void CancelTemporaryIcons()
        {
            if (mapControl.MapElements.Count() > _markerService.GetAll().Count())
            {
                mapControl.MapElements.RemoveAt(mapControl.MapElements.Count() - 1);
            }
        }

        private void DeleteTextboxes()
        {
            lab.Text = string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _border.Visibility = Visibility.Collapsed;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MapViewModel)DataContext;
            vm.SelectedItem = 0;
            Combo.Text = "";
        }
    }
}