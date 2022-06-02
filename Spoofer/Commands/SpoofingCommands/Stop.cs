using Spoofer.Commands.UserCommands;
using Spoofer.Services.Spoofer;
using Spoofer.ViewModels;
using System;
using System.Windows;

namespace Spoofer.Commands.SpoofingCommands
{
    public class Stop : BaseCommand
    {
        private readonly ISpooferService _spoofer;
        private readonly ViewModelBase _mapViewModel;
        public Stop(ISpooferService spoofer, ViewModelBase mapViewModel)
        {
            _spoofer = spoofer;
            _mapViewModel = mapViewModel;
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public override void Execute(object parameter)
        {
            try
            {
                _spoofer.StopTransmitting(_mapViewModel);
                MessageBox.Show("Tx Is Off");

            }
            catch (Exception ex)
            {

            }
        }
    }
}
