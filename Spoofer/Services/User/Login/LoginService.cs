using Spoofer.Data;
using Spoofer.Services.Navigation;
using Spoofer.ViewModels;
using System;
using System.Linq;
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
            model.ErrorMessageViewModel.ErrorMessage = "";
            model.IsLoading = true;

            if (!_context.User.Any(p => p.UserName == model.UserName && p.Password == model.Password))
            {

                throw new Exception("Username Or Password are Incorrect");
                model.IsLoading = false;

            }
            else
            {
                _navigation.Navigate();
            }

        }
    }
}