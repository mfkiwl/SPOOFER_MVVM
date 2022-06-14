using log4net;
using Spoofer.Commands.UserCommands;
using Spoofer.Exceptions;
using Spoofer.Services.Marker;
using Spoofer.Services.Spoofer;
using Spoofer.ViewModels;
using System;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Windows;

namespace Spoofer.Commands.Spoofing
{
    public class Transmit : BaseCommand
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMarkerService _marker;
        private readonly ISpooferService _spoofer;
        private readonly MapViewModel _mapViewModel;
        public Transmit(IMarkerService marker, ISpooferService spoofer, MapViewModel mapViewModel)
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
                _mapViewModel.ErrorMessageViewModel.Refresh();
                _spoofer.TransmitFromFile(_mapViewModel);
                MessageBox.Show("Tx Is On");
            }
            catch (CoordinateNotExistException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                log.Error(ex.Message);
            }
            catch (FileNotExistException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                log.Error(ex.Message);
            }
            catch (PingException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                log.Error(ex.Message);
            }
            catch (SDRException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error("Unexpected", ex);
            }

        }
    }
}
