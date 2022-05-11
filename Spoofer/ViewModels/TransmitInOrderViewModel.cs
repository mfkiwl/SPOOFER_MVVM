using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Spoofer.ViewModels
{
    public class TransmitInOrderViewModel : ViewModelBase
    {
        public ObservableCollection<CoordinateViewModel> Coordinates { get; set; }
        public string Duration { get; set; }
        public ICommand Transmit {get;}
        public ICommand Stop {get;}
        public ICommand RemoveFromList {get;}
       
    }
}
