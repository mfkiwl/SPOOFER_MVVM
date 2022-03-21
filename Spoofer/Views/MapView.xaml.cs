using Spoofer.Services.Marker;
using System.Windows.Media.Imaging;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
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

        public MapView()
        {
            InitializeComponent();
            _markerService = new MarkerService(App._context);
            
        }

        private void MapControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
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
                    Image = RandomAccessStreamReference.CreateFromUri(new System.Uri("C:/Users/max/source/repos/Spoofer/Spoofer/Assets/icon.png")),
                    ZIndex = 0
                };
                mapControl.MapElements.Add(mapIcon);
                
            }
        }

        private void MapControl_MapElementClick(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.MapElementClickEventArgs e)
        {
            foreach (var CONTROL in mapControl.MapElements)
            {

            }
        }

        private void MapControl_MapTapped(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.MapInputEventArgs e)
        {

        }
    }
}
