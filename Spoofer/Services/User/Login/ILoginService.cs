using Spoofer.ViewModels;

namespace Spoofer.Services.User
{
    public interface ILogin
    {
        void OnLogin(LoginViewModel model);
    }
}