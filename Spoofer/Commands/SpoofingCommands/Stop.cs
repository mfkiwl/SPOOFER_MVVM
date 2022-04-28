﻿using Spoofer.Commands.UserCommands;
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
    public class Stop : BaseCommand
    {
        private readonly ISpoofer _spoofer;
        private readonly MapViewModel _mapViewModel;
        public Stop(ISpoofer spoofer, MapViewModel mapViewModel)
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
            catch(Exception ex)
            {

            }
        }
    }
}
