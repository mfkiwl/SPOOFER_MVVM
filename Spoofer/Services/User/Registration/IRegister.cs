using Spoofer.ViewModels;

namespace Spoofer.Services.User
{
    public interface IRegister
    {
        void OnRegister(AccountViewModel model);
    }
}