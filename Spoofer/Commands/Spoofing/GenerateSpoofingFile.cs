using Spoofer.Commands.UserCommands;
using Spoofer.EXMethods;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Commands.Spoofing
{
    public class GenerateSpoofingFile : BaseCommand
    {
       
        private readonly MapViewModel _mapViewModel;
        public GenerateSpoofingFile(MapViewModel mapViewModel)
        {
            _mapViewModel = mapViewModel;
        }
        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter);
        }
        public override void Execute(object parameter)
        {
            
        }
        
    }
}
