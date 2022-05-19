using GongSolutions.Wpf.DragDrop;
using Spoofer.Commands.MarkersCommands;
using Spoofer.Commands.SpoofingCommands;
using Spoofer.Models;
using Spoofer.Services.Marker;
using Spoofer.Services.Spoofer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Spoofer.ViewModels
{
    public class TransmitInOrderViewModel : ViewModelBase, IDropTarget
    {
        private readonly ISpooferService _spoofer;
        private readonly IMarkerService _service;
        public TransmitInOrderViewModel(IMarkerService service, ISpooferService spoofer)
        {
            _service = service;
            _spoofer = spoofer;
            ErrorMessageViewModel = new MessageViewModel();
            Navigate = new Navigate(_service, this);
            coordinates = new ObservableCollection<CoordinateViewModel>();
            durationList = new ObservableCollection<int>();
            setDurations();
            Coordinates = CollectionViewSource.GetDefaultView(UpdateData());
            Coordinates.SortDescriptions.Add(new SortDescription(nameof(CoordinateViewModel.NumberInOrder), ListSortDirection.Ascending));
            Transmit = new TransmitInOrderCommand(_spoofer, this);
            Stop = new Stop(_spoofer, this);
            GenerateInOrder = new GenerateOrderFile(this, _spoofer);

        }
        private ObservableCollection<CoordinateViewModel> coordinates;
        private ObservableCollection<int> durationList;
        public ObservableCollection<int> DurationList { get { return durationList; } set { durationList = value; OnPropertyChanged(nameof(DurationList)); } }
        public ICollectionView Coordinates { get; }

        private int duration;

        public int Duration
        {
            get { return duration; }
            set { duration = value; OnPropertyChanged(nameof(Duration)); }
        }

        private bool _isTransmitting;

        public bool IsTransmitting
        {
            get { return _isTransmitting; }
            set { _isTransmitting = value; OnPropertyChanged(nameof(IsTransmitting)); }
        }
        private string _locationTransmitted;

        public string LocationTransmitted
        {
            get { return _locationTransmitted; }
            set
            {
                _locationTransmitted = value;
                OnPropertyChanged(nameof(LocationTransmitted));
            }
        }

        public MessageViewModel ErrorMessageViewModel { get; }
        public ICommand Transmit { get; }
        public ICommand Stop { get; }
        public ICommand RemoveFromList { get; }
        public ICommand GenerateInOrder { get; }
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
            for (int i = 30; i <= 120; i = i + 15)
            {
                durationList.Add(i);
            }
        }

        public void DragEnter(IDropInfo dropInfo)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.All;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.All;


        }
        public void DragLeave(IDropInfo dropInfo)
        {
            return;
        }


        public void Drop(IDropInfo dropInfo)
        {

            var sourceItem = dropInfo.Data as CoordinateViewModel;
            var realcooSource = _service.GetAll().SingleOrDefault(c => c.NumberInOrder == sourceItem.NumberInOrder);
            if (dropInfo.TargetItem is CoordinateViewModel)
            {
                var targetItem = dropInfo.TargetItem as CoordinateViewModel;
                var realcootarget = _service.GetAll().SingleOrDefault(c => c.NumberInOrder == targetItem.NumberInOrder);
                coordinates.Clear();
                _service.UpdateAfterDrop(realcooSource, realcootarget);
                dropInfo.Effects = DragDropEffects.All;
                dropInfo.EffectText = "Drop Here";
            }
           
            else
            {
                coordinates.Clear();
                _service.RemoveFromList(realcooSource);
            }
            UpdateData();
        }


    }
}
