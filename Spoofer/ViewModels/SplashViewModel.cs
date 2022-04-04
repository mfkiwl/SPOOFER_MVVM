using Spoofer.Commands.Spoofing;
using Spoofer.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Spoofer.ViewModels
{
    public class SplashViewModel: ViewModelBase
    {
        private readonly NavigationService _navigationService;
        public ICommand GoBack { get; }
        public SplashViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            GoBack = new GenerateSpoofingFile(_navigationService);
        }
    }
}
