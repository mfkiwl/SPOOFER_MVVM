using log4net;
using Spoofer.Services.User;
using Spoofer.ViewModels;
using System;
using System.Threading.Tasks;

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
            _accountViewModel.PropertyChanged += _accountViewModel_PropertyChanged;
        }

        private void _accountViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_accountViewModel.UserName) || e.PropertyName == nameof(_accountViewModel.Password))
            {
                OnCanExecuteChange();
            }
        }

        //public override bool CanExecute(object parameter)
        //{
        //    return !String.IsNullOrEmpty(_accountViewModel.UserName) &&
        //        !String.IsNullOrEmpty(_accountViewModel.Password);
        //}

        public override void Execute(object parameter)
        {
            try
            {
                _accountViewModel.IsLoading = true;
                Task.Run(() => login());
                
                log.Info($"User {_accountViewModel.UserName} Logged In Seccesfully!!!!!");

            }
            catch(ArgumentException ex)
            {
                _accountViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
            }
            catch (Exception e)
            {
                log.Error("Can't log in Exception");
            }
        }
        private void login()
        {
            _login.OnLogin(_accountViewModel);
        }
    }
}