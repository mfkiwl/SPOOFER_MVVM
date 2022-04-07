using log4net;
using Spoofer.Services.User;
using Spoofer.ViewModels;
using System;

namespace Spoofer.Commands.UserCommands
{
    public class RegisterCommand : BaseCommand
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IRegister _iRegister;
        private readonly AccountViewModel _accountViewModel;

        public RegisterCommand(IRegister iRegister, AccountViewModel accountViewModel)
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
            try
            {
                _iRegister.OnRegister(_accountViewModel);
                log.Info("User Registered Succesfully");
            }
            catch (Exception e)
            {
                log.Error("Cant Register Exception!!!!!", e);
            }
        }
    }
}