using Spoofer.Commands.MarkersCommand;
using Spoofer.Commands.MarkersCommands;
using Spoofer.Commands.Spoofing;
using Spoofer.Commands.SpoofingCommands;
using Spoofer.Commands.UserCommands;
using Spoofer.Services.Marker;
using Spoofer.Services.Spoofer;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Spoofer.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        public MapViewModel(IMarkerService service, ISpooferService spoofer)
        {
            IsLoading = false;
            _service = service;
            _spoofer = spoofer;
            IsPinging = BaseCommand.PingHost("10.0.0.41");
            Add = new AddMark(this, _service);
            numberInOrder = new ObservableCollection<int>();
            updateCollection();
            Remove = new RemoveMark(this, _service);
            GenerateFile = new Generate(_service, _spoofer, this);
            TransmitNow = new Transmit(_service, _spoofer, this);
            StopTransmit = new Stop(_spoofer, this);
            Navigate = new Navigate(_service, this);
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

        public double? Height
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

        private ObservableCollection<int> numberInOrder;

        public ObservableCollection<int> NumbersInOrder { get { return numberInOrder; } set { numberInOrder = value; OnPropertyChanged(nameof(NumbersInOrder)); } }
        private int _selectedItem;

        public int? SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = (int)value; OnPropertyChanged(nameof(HasValue)); OnPropertyChanged(nameof(SelectedItem)); }
        }

        public bool HasValue => SelectedItem >= 1;

        public MessageViewModel ErrorMessageViewModel { get; }
        public ICommand GenerateFile { get; }

        public ICommand Add { get; }
        public ICommand Remove { get; }

        public ICommand TransmitNow { get; }
        public ICommand StopTransmit { get; }
        public ICommand Navigate { get; }


        public void updateCollection()
        {
            for (int i = 1; i < 6; i++)
            {
                numberInOrder.Add(i);
            }
        }


    }
}