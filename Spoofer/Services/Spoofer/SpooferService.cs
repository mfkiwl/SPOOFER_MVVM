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
        private int counter = 0;
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
                if (counter == 0)
                {
                    var argc = argv.Length;
                    EXMethods.SpoofingMethods.main(argc, argv);
                    counter++;
                }
                else
                {
                    var process = new Process();
                    proccess.StartInfo.FileName = "Core.dll";
                    proccess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    proccess.StartInfo.Arguments = $"-e brdc0620.22n -s 2500000 -l {argv[6]} -o {argv[8]}";
                    process.Start();
                }
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
            if (!BaseCommand.PingHost(IP_ADDRESS, viewModel))
            {
                throw new PingException($"{IP_ADDRESS} is not connected");
            }
            else
            {
                proccess.StartInfo.FileName = "tx_samples_from_file";
                proccess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proccess.StartInfo.RedirectStandardInput = true;
                proccess.StartInfo.RedirectStandardOutput = false;
                proccess.StartInfo.Arguments = $@"--file {viewModel.Label.Trim()}.bin --type short --rate 2500000 --freq 1575420000 --gain 20 --repeat --ref external";
                proccess.StartInfo.UseShellExecute = false;
                proccess.Start();
                viewModel.IsTransmitting = true;
                if (proccess.HasExited)
                {
                    viewModel.IsTransmitting = false;
                }
            }
        }
        public void StopTransmitting(MapViewModel viewModel)
        {
            proccess.Kill();
            viewModel.IsTransmitting = false;
        }
    }
}
