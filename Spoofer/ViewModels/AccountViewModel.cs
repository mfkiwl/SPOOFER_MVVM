using Spoofer.Commands.UserCommands;
using Spoofer.Services.User;
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
            Login = new LoginCommand(_iLogin, this);
            Register = new RegisterCommand(_iRegister, this);
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(nameof(Password)); }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(nameof(UserName)); }
        }

        private bool isStayedLoggedIn;

        public bool IsStayedLoggedIn
        {
            get { return isStayedLoggedIn; }
            set { isStayedLoggedIn = value; OnPropertyChanged(nameof(IsStayedLoggedIn)); }
        }

        public ICommand Login { get; }
        public ICommand Register { get; }
    }
}