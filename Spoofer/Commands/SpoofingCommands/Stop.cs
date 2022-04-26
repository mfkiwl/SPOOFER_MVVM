using Spoofer.Commands.UserCommands;
using Spoofer.Services.Spoofer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Commands.SpoofingCommands
{
    public class Stop : BaseCommand
    {
        private readonly ISpoofer _spoofer;
        public Stop(ISpoofer spoofer)
        {
            _spoofer = spoofer;
        }
        public override void Execute(object parameter)
        {
            _spoofer.StopTransmitting();
        }
    }
}
