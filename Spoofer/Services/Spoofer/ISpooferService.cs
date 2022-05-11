using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Services.Spoofer
{
    public interface ISpooferService
    {
        void GenerateIQFile(string [] argv, MapViewModel viewModel);
        void TransmitFromFile(MapViewModel viewModel);
        void StopTransmitting(MapViewModel viewModel);
        
    }
}
