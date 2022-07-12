using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.ViewModels.DTO
{
    public class UserFormViewModel : ViewModelBase
    {
        private string _userName;

        public string Username
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(nameof(Username)); }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public IEnumerable<Permission> Permission
        {
            get { return Enum.GetValues(typeof(Permission)).Cast<Permission>(); }
        }


    }
public enum Permission
{
    SimpleUser,
    Admin,
    SuperUser
}
}
