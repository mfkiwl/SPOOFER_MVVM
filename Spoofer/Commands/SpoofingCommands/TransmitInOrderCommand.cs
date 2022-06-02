using Spoofer.Commands.UserCommands;
using Spoofer.Exceptions;
using Spoofer.Services.Spoofer;
using Spoofer.ViewModels;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Spoofer.Commands.SpoofingCommands
{
    public class TransmitInOrderCommand : BaseCommand
    {
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
            }
            catch (PingException ex)
            {
                _transmitInOrderViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
            }
            catch (SDRException ex)
            {
                _transmitInOrderViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
            }
        }
    }
}
