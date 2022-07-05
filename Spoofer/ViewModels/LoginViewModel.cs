using Spoofer.Commands.UserCommands;
using Spoofer.Services.User;
using Spoofer.Services.User.Register;
using System;
using System.Reflection;
using System.Windows.Input;

namespace Spoofer.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ILogin _iLogin;
        private readonly IRegister _register;

        public LoginViewModel(ILogin iLogin, IRegister register)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            Description = $"Software Version  {version}\nCopyright © 1996-{DateTime.Now.ToString("yyyy")}";
            _iLogin = iLogin;
            _register = register;
            ErrorMessageViewModel = new MessageViewModel();
            Login = new LoginCommand(_iLogin, this);
            Register = new RegisterCommand(_register, this);
        }
        public string Description { get; set; }

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
        public MessageViewModel ErrorMessageViewModel { get; }
        public ICommand Login { get; }
        public ICommand Register { get; }
    }
}