using Spoofer.Services.User;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Commands.UserCommands
{
    public class LoginCommand : BaseCommand
    {
        private readonly AccountViewModel _accountViewModel;
        private readonly ILogin _login;
        public LoginCommand(ILogin login, AccountViewModel accountViewModel)
        {
            _accountViewModel = accountViewModel;
            _login = login;
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
                !String.IsNullOrEmpty(_accountViewModel.Password);
        }
        public override void Execute(object parameter)
        {
            _login.OnLogin(_accountViewModel);
        }
    }
}
