using Spoofer.Commands.UserCommands;
using Spoofer.Exceptions;
using Spoofer.EXMethods;
using Spoofer.Services.Marker;
using Spoofer.Services.Spoofer;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Spoofer.Commands.Spoofing
{
    public class Transmit : BaseCommand
    {
       
        private readonly IMarkerService _marker;
        private readonly ISpoofer _spoofer;
        private readonly MapViewModel _mapViewModel;
        public Transmit(IMarkerService marker, ISpoofer spoofer, MapViewModel mapViewModel)
        {
            _marker = marker;
            _spoofer = spoofer;
            _mapViewModel = mapViewModel;
            _mapViewModel.PropertyChanged += ViewModelPropertyChanged;
        }
        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter) && !_mapViewModel.IsTransmitting;
        }
        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChange();
        }

        public override void Execute(object parameter)
        {
            try
            {
                _spoofer.TransmitFromFile(_mapViewModel);
                MessageBox.Show("Tx Is On");
            }
            catch(CoordinateNotExistException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
            }
            catch(FileNotExistException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
            }
            catch(PingException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
