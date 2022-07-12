using Spoofer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.ViewModels.DTO
{
    public class UserDTOViewModel : ViewModelBase
    {
        private readonly User _user;
        public UserDTOViewModel(User user)
        {
            _user = user;
        }
        public string Id => _user.Id;
        public string Username => _user.UserName;
        public string Permission => _user.Permission;
        public string Password => _user.Password;
    }
}
