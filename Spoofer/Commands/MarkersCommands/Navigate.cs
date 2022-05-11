using Spoofer.Commands.UserCommands;
using Spoofer.Services.Marker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Commands.MarkersCommands
{
    public class Navigate : BaseCommand
    {
        private readonly IMarkerService _service;
        public Navigate(IMarkerService service)
        {
            _service = service;
        }
        public override void Execute(object parameter)
        {
            _service.Navigate();
        }
    }
}
