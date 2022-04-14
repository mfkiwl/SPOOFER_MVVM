using Spoofer.Commands.UserCommands;
using Spoofer.Services.User;
using System;
using System.Windows.Input;

namespace Spoofer.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        private readonly IRegister _iRegister;
        private readonly ILogin _iLogin;

        public AccountViewModel(IRegister iRegister, ILogin iLogin)
        {
            _iRegister = iRegister;
            _iLogin = iLogin;
            ErrorMessageViewModel = new MessageViewModel();
            Login = new LoginCommand(_iLogin, this);
            Register = new RegisterCommand(_iRegister, this);
        }

        private string password;
       
        public string Password
        {
            get { return password; }
            set 
            {
               
                password = value;
                OnPropertyChanged(nameof(Password)); 
            }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set 
            {
               
                userName = value;
                OnPropertyChanged(nameof(UserName)); 
            }
        }
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set 
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        public MessageViewModel ErrorMessageViewModel { get;}
        public ICommand Login { get; }
        public ICommand Register { get; }
    }
}