using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Spoofer.Commands.MarkersCommand;
using Spoofer.Commands.Spoofing;
using Spoofer.Services.Marker;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Devices.Geolocation;
using static System.Net.Mime.MediaTypeNames;

namespace Spoofer.ViewModels
{
    public class MapViewModel : ViewModelBase
    {

        private readonly IMarkerService _service;

        private string _label;

        public string Label
        {
            get { return _label; }
            set { _label = value; OnPropertyChanged("Label"); }
        }

        private double _latitude;

        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; OnPropertyChanged("Latitude"); }
        }

        private double _longitude;

        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; OnPropertyChanged("Longitude"); }
        }

        private double _height;

        public Nullable<double> Height
        {
            get { return _height; }
            set { _height = (double)value; OnPropertyChanged("Height"); }
        }

        private Windows.Devices.Geolocation.Geopoint _selectedMarkerId;

        public Windows.Devices.Geolocation.Geopoint SelectedMarkerId
        {
            get { return _selectedMarkerId; }
            set { _selectedMarkerId = value; OnPropertyChanged("SelectedMarkerId"); }
        }
        public ICommand GenerateFile { get; }
        
        public ICommand Add { get; }
        public ICommand Remove { get; }

        public ICommand GetMark { get; }

        public ICommand GetAllMarks { get; }

        public MapViewModel(IMarkerService service)
        {
            _service = service;
            Add = new AddMark(this, _service);
            Remove = new RemoveMark(this, _service);
        }

    }
}