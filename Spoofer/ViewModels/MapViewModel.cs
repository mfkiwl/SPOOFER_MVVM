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
using System.IO;
using Spoofer.Services.Spoofer;
using Spoofer.Commands.SpoofingCommands;
using Spoofer.Commands.UserCommands;
using System.Linq;

namespace Spoofer.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        public MapViewModel(IMarkerService service, ISpooferService spoofer)
        {
            _service = service;
            _spoofer = spoofer;
            BaseCommand.PingHost("10.0.0.41", this);
            numberInOrder = new ObservableCollection<int>();
            Add = new AddMark(this, _service);
            Remove = new RemoveMark(this, _service);
            GenerateFile = new Generate(_service, _spoofer, this);
            TransmitNow = new Transmit(_service, _spoofer, this);
            StopTransmit = new Stop(_spoofer, this);
            ErrorMessageViewModel = new MessageViewModel();

        }
        private bool _isPinging;

        public bool IsPinging
        {
            get { return _isPinging; }
            set { _isPinging = value; OnPropertyChanged(nameof(IsPinging)); }
        }

        private readonly IMarkerService _service;
        private readonly ISpooferService _spoofer;

        private string _label;

        public string Label
        {
            get { return _label; }
            set { _label = value; OnPropertyChanged(nameof(Label)); }
        }

        private double _latitude;

        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; OnPropertyChanged(nameof(Latitude)); }
        }

        private double _longitude;

        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; OnPropertyChanged(nameof(Longitude)); }
        }

        private double _height;

        public Nullable<double> Height
        {
            get { return _height; }
            set { _height = (double)value; OnPropertyChanged(nameof(Height)); }
        }
        private bool _isFileCreated;

        public bool IsFileCreated { get { return _isFileCreated; } set { _isFileCreated = value; OnPropertyChanged(nameof(IsFileCreated)); } }
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }
        private bool _isFinishLoading = true;

        public bool IsFinishLoading
        {
            get { return _isFinishLoading; }
            set { _isFinishLoading = value; OnPropertyChanged(nameof(IsFinishLoading)); }
        }
        private bool _isTransmitting;

        public bool IsTransmitting
        {
            get { return _isTransmitting; }
            set { _isTransmitting = value; OnPropertyChanged(nameof(IsTransmitting)); }
        }
        private bool _isUpToDate;
        private ObservableCollection<int> numberInOrder;

        public ObservableCollection<int> NumberInOrder => numberInOrder;
        public int SelectedItem { get; set; }


        public MessageViewModel ErrorMessageViewModel { get; }
        public ICommand GenerateFile { get; }

        public ICommand Add { get; }
        public ICommand Remove { get; }

        public ICommand TransmitNow { get; }
        public ICommand StopTransmit { get; }
        public ObservableCollection<int> updateCollection()
        {
            numberInOrder.Clear();
            for (int i = 0; i < _service.GetAll().Count(); i++)
            {

                if (_service.GetAll().ToList()[i].HasFile)
                {
                    numberInOrder.Add(i);
                }
            }
            return numberInOrder;
        }

    }
}