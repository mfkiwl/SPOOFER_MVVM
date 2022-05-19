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
    public class GenerateOrderFile :BaseCommand
    {
        private readonly TransmitInOrderViewModel viewModel;
        private readonly ISpooferService spoofer;

        public GenerateOrderFile(TransmitInOrderViewModel viewModel, ISpooferService spoofer)
        {
            this.viewModel = viewModel;
            this.spoofer = spoofer;
        }

        public override void Execute(object parameter)
        {
            spoofer.GenerateInOrder(viewModel);
        }
    }
}
