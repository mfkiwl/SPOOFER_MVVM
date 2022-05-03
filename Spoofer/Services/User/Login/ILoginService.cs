using Spoofer.ViewModels;

namespace Spoofer.Services.User
{
    public interface ILogin
    {
        void OnLogin(AccountViewModel model);
    }
}