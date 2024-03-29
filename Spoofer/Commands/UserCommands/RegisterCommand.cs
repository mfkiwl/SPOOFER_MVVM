﻿using Spoofer.Services.User.Register;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Spoofer.Commands.UserCommands
{
    public class RegisterCommand : BaseCommand
    {
        private readonly IRegister _iRegister;
        private readonly LoginViewModel _accountViewModel;

        public RegisterCommand(IRegister iRegister, LoginViewModel accountViewModel)
        {
            _iRegister = iRegister;
            _accountViewModel = accountViewModel;
            _accountViewModel.PropertyChanged += _accountViewModel_PropertyChanged;
        }

        private void _accountViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_accountViewModel.UserName) || e.PropertyName == nameof(_accountViewModel.Password))
            {
                OnCanExecuteChange();
            }
        }

        public override bool CanExecute(object parameter)
        {
            return !String.IsNullOrEmpty(_accountViewModel.UserName) &&
                !String.IsNullOrEmpty(_accountViewModel.Password) && base.CanExecute(parameter);
        }

        public override void Execute(object parameter)
        {
            _iRegister.OnRegister(_accountViewModel);
            MessageBox.Show("User registered Succesfully");
        }
    }
}
