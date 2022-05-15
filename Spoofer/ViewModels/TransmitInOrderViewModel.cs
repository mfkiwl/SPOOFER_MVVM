using Spoofer.Commands.MarkersCommands;
using Spoofer.Commands.SpoofingCommands;
using Spoofer.Services.Marker;
using Spoofer.Services.Spoofer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Spoofer.ViewModels
{
    public class TransmitInOrderViewModel : ViewModelBase
    {
        private readonly ISpooferService _spoofer;
        private readonly IMarkerService _service;
        public TransmitInOrderViewModel(IMarkerService service, ISpooferService spoofer)
        {
            _service = service;
            _spoofer = spoofer;
            Navigate = new Navigate(_service);
            coordinates = new ObservableCollection<CoordinateViewModel>();
            durationList = new ObservableCollection<int>();
            setDurations();
            Coordinates = CollectionViewSource.GetDefaultView(UpdateData());
            Coordinates.SortDescriptions.Add(new SortDescription(nameof(CoordinateViewModel.NumberInOrder), ListSortDirection.Ascending));
            Transmit = new TransmitInOrderCommand(_spoofer, this);
            ErrorMessageViewModel = new MessageViewModel();
        }
        private ObservableCollection<CoordinateViewModel> coordinates;
        private ObservableCollection<int> durationList;
        public ObservableCollection<int> DurationList { get { return durationList; } set {durationList = value; OnPropertyChanged(nameof(DurationList)); } }
        public ICollectionView Coordinates { get; }
        
        private int duration;

        public int Duration
        {
            get { return duration; }
            set { duration = value; OnPropertyChanged(nameof(Duration)); }
        }
        public MessageViewModel ErrorMessageViewModel { get; }
        public ICommand Transmit { get; }
        public ICommand Stop { get; }
        public ICommand RemoveFromList { get; }
        public ICommand Navigate { get; }
        public ObservableCollection<CoordinateViewModel> UpdateData()
        {
            var list = _service.GetAll();
            foreach (var coordinate in list)
            {
                if (coordinate.NumberInOrder != null)
                {
                    var viewModel = new CoordinateViewModel(coordinate);
                    coordinates.Add(viewModel);
                }
            }
            return coordinates;
        }
        private void setDurations()
        {
            for (int i = 30; i < 120; i=i+15)
            {
                durationList.Add(i);
            }
        }

    }
}
