using Spoofer.Commands.UserCommands;
using Spoofer.Exceptions;
using Spoofer.Services.Spoofer;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Spoofer.Commands.SpoofingCommands
{
    public class GenerateOrderFile : BaseCommand
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
            try
            {
                spoofer.GenerateInOrder(viewModel);
                MessageBox.Show($"Streak File is ready");
            }
            catch(FileNotExistException ex)
            {
                viewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
