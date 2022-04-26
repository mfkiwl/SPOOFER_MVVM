using Spoofer.Commands.UserCommands;
using Spoofer.Data;
using Spoofer.Exceptions;
using Spoofer.Services.Marker;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Services.Spoofer
{
    public class SpooferService : ISpoofer
    {
        private const string IP_ADDRESS = "10.0.0.41";
        private readonly Process proccess;
        private readonly CoordinatesContext _context;
        private readonly IMarkerService _marker;

        public SpooferService(CoordinatesContext context, IMarkerService marker)
        {
            proccess = new Process();
            _context = context;
            _marker = marker;
        }
        public void GenerateIQFile(string[] argv, MapViewModel viewModel)
        {
            if (!_marker.isExist(viewModel))
            {
                throw new CoordinateNotExistException();
            }
            if (BaseCommand.isFileExist(viewModel))
            {
                throw new FileAlreadyExistException();
            }
            else
            {
                var argc = argv.Length;
                EXMethods.SpoofingMethods.main(argc, argv);
            }
        }
        public void TransmitFromFile(MapViewModel viewModel)
        {
            if (!BaseCommand.isFileExist(viewModel))
            {
                throw new FileNotExistException();
            }
            if (!_marker.isExist(viewModel))
            {
                throw new CoordinateNotExistException();
            }
            if (!BaseCommand.PingHost(IP_ADDRESS))
            {
                throw new PingException($"{IP_ADDRESS} is not connected");
            }
            else
            {
                proccess.StartInfo.FileName = "tx_samples_from_file";
                proccess.StartInfo.UseShellExecute = false;
                proccess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proccess.StartInfo.Arguments = $@"--file {viewModel.Label.Trim()}.bin --type short --rate 2500000 --freq 1575420000 --gain 31.5 --repeat ";
                proccess.Start();
            }
        }
        public void StopTransmitting()
        {
            proccess.Kill();
        }
    }
}
