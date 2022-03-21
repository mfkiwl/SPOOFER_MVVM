using Spoofer.Services.Navigation;
using Spoofer.ViewModels;
using Spoofer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Services.User
{
    public class ServiceLogin : ILogin
    {
        private readonly CoordinatesContext _context;
        private readonly NavigationService _navigation;
        public ServiceLogin(CoordinatesContext context, NavigationService navigation)
        {
            _context = context;
            _navigation = navigation;

        }
        public void OnLogin(AccountViewModel model)
        {
            foreach (var user in _context.User)
            {
                if (model.UserName == user.UserName)
                {
                    var userLog = user;
                    if (userLog.Password == model.Password)
                    {
                        _navigation.Navigate();
                        
                    }
                }
            }

        }
    }


}
