using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Spoofer.Commands.MarkersCommand;
using Spoofer.Services.Marker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;



namespace Spoofer.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        private readonly IMarkerService _service;
        public ICollectionView Markers { get; set; }

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

        private string _selectedMarkerId;

        public string SelectedMarkerId
        {
            get { return _selectedMarkerId; }
            set { _selectedMarkerId = value; OnPropertyChanged("SelectedMarkerId"); }
        }

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
