using Spoofer.Data;
using Spoofer.Services.Navigation;
using Spoofer.ViewModels;
using System;
using System.Threading;
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
            model.IsLoading = true;
            model.ErrorMessageViewModel.ErrorMessage = "";
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
                model.IsLoading = false;
                model.ErrorMessageViewModel.ErrorMessage = "Username Or Password are Incorrect";
            }
        }
    }
}