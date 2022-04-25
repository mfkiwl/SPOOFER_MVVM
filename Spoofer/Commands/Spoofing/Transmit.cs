using Spoofer.Commands.UserCommands;
using Spoofer.EXMethods;
using Spoofer.Services.Marker;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Commands.Spoofing
{
    public class Transmit : BaseCommand
    {
        private readonly IMarkerService _marker;
        private readonly MapViewModel _mapViewModel;
        public Transmit(IMarkerService marker, MapViewModel mapViewModel)
        {
            _marker = marker;
            _mapViewModel = mapViewModel;
            _mapViewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChange();
        }

        public override void Execute(object parameter)
        {
            Process process = new Process();
            process.StartInfo.FileName = "tx_samples_from_file.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.Arguments = $@"--file {_mapViewModel.Label.Trim()}.bin --type short --rate 2500000 --freq 1575420000 --gain 31.5 --repeat ";
            process.Start();
        }
    }
}
