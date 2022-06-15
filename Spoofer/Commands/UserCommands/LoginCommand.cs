using log4net;
using Spoofer.Exceptions;
using Spoofer.Services.User;
using Spoofer.ViewModels;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace Spoofer.Commands.UserCommands
{
    public class LoginCommand : BaseCommand
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly AccountViewModel _accountViewModel;
        private readonly ILogin _login;

        public LoginCommand(ILogin login, AccountViewModel accountViewModel)
        {
            _accountViewModel = accountViewModel;
            _login = login;
        }

        private void _accountViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_accountViewModel.UserName) || e.PropertyName == nameof(_accountViewModel.Password))
            {
                OnCanExecuteChange();
            }
        }



        public override void Execute(object parameter)
        {
            try
            {
                Task.Run(() => _login.OnLogin(_accountViewModel));
                _accountViewModel.IsLoading = true;
                log.Info($"User {_accountViewModel.UserName} Logged In Seccesfully!!!!!");

            }
            catch (FileNotExistException ex)
            {
                _accountViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                _accountViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                log.Error("Can't log in", ex);
            }

        }

    }
}