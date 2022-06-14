using log4net;
using Spoofer.Commands.UserCommands;
using Spoofer.Exceptions;
using Spoofer.Services.Spoofer;
using Spoofer.ViewModels;
using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;

namespace Spoofer.Commands.SpoofingCommands
{
    public class TransmitInOrderCommand : BaseCommand
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ISpooferService _spoofer;
        private readonly TransmitInOrderViewModel _transmitInOrderViewModel;

        public TransmitInOrderCommand(ISpooferService spoofer, TransmitInOrderViewModel transmitInOrderViewModel)
        {
            _spoofer = spoofer;
            _transmitInOrderViewModel = transmitInOrderViewModel;
        }
        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter) && !_transmitInOrderViewModel.IsTransmitting;
        }
        public async override void Execute(object parameter)
        {
            try
            {
                await Task.Run(() => _spoofer.TransmitInOrder(_transmitInOrderViewModel));
            }
            catch (FileNotExistException ex)
            {
                _transmitInOrderViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                log.Error(ex.Message);
            }
            catch (PingException ex)
            {
                _transmitInOrderViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                log.Error(ex.Message);

            }
            catch (SDRException ex)
            {
                _transmitInOrderViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                log.Error(ex.Message);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error("Unhandled", ex);
            }
        }
    }
}
