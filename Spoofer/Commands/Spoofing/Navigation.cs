using Spoofer.Commands.UserCommands;
using Spoofer.EXMethods;
using Spoofer.Services.Navigation;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Commands.Spoofing
{
    public class Navigation : BaseCommand
    {

        private readonly NavigationService _navigationService;
        public Navigation(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
        
    }
}
