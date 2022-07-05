using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Services.User.Register
{
    public interface IRegister
    {
        void OnRegister(LoginViewModel model);
    }
}
