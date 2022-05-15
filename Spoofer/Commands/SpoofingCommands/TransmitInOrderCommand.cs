using Spoofer.Commands.UserCommands;
using Spoofer.Services.Spoofer;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public override void Execute(object parameter)
        {
            try
            {
                Task.Run(() => _spoofer.TransmitInOrder(_transmitInOrderViewModel));
            }
            catch(Exception ex)
            {
                _transmitInOrderViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
            }
        }
    }
}
