using log4net;
using Spoofer.Commands.UserCommands;
using Spoofer.Data;
using Spoofer.Exceptions;
using Spoofer.Services.Marker;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Spoofer.Services.Spoofer
{
    public class SpooferService : ISpooferService
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string IP_ADDRESS = "10.0.0.41";
        private readonly Process proccess;
        private int counter = 0;
        private readonly IMarkerService _marker;

        public SpooferService( IMarkerService marker)
        {
            proccess = new Process();
            _marker = marker;
        }
        /// <summary>
        /// Initializes I/Q File for the location The User Spotted On The Map.
        /// </summary>
        /// <param name="argv">
        /// Represents the parameters of the file CMD.   
        /// </param>
        /// <param name="viewModel">
        /// Represents the Location the user create the spoofing file for.
        /// </param>
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
                switch (counter)
                {
                    case 0:
                        EXMethods.SpoofingMethods.main(argc, argv);
                        break;
                    case 1:
                        EXMethods.SpoofingMethods2.main(argc, argv);
                        break;
                    case 2:
                        EXMethods.SpoofingMethods3.main(argc, argv);
                        break;
                    case 3:
                        EXMethods.SpoofingMethods4.main(argc, argv);
                        break;
                    case 4:
                        EXMethods.SpoofingMethods5.main(argc, argv);
                        break;
                    case 5:
                        EXMethods.SpoofingMethods6.main(argc, argv);
                        break;
                    case 6:
                        EXMethods.SpoofingMethods7.main(argc, argv);
                        break;
                    case 7:
                        EXMethods.SpoofingMethods8.main(argc, argv);
                        break;
                }
                counter++;
            }
        }
        /// <summary>
        /// Updating All The Saved Files for the current datetime when initializing
        /// </summary>
        /// <param name="argv"></param>
        public void GenerateIQFile(string[] argv)
        {
            var argc = argv.Length;
            switch (counter)
            {
                case 0:
                    EXMethods.SpoofingMethods2.main(argc, argv);
                    break;
                case 1:
                    EXMethods.SpoofingMethods.main(argc, argv);
                    break;
                case 2:
                    EXMethods.SpoofingMethods3.main(argc, argv);
                    break;
                case 3:
                    EXMethods.SpoofingMethods4.main(argc, argv);
                    break;
                case 4:
                    EXMethods.SpoofingMethods5.main(argc, argv);
                    break;
                case 5:
                    EXMethods.SpoofingMethods6.main(argc, argv);
                    break;
                case 6:
                    EXMethods.SpoofingMethods7.main(argc, argv);
                    break;
                case 7:
                    EXMethods.SpoofingMethods8.main(argc, argv);
                    break;
            }
            counter++;

        }
        /// <summary>
        /// Transmition of specific Location On The Map.
        /// </summary>
        /// <param name="viewModel"></param>
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
                viewModel.IsPinging = false;
                throw new PingException($"{IP_ADDRESS} is not connected");
            }
            else
            {
                transmit(viewModel.Label);
                viewModel.IsTransmitting = true;
                log.Info("System is Transmitting");

                if (proccess.HasExited)
                {
                    viewModel.IsTransmitting = false;
                    log.Info("System Stop Transmitting");
                }
            }
        }
        /// <summary>
        /// Stop the Transmition.
        /// </summary>
        /// <param name="viewModel"></param>
        public void StopTransmitting(ViewModelBase viewModel)
        {
            proccess.Kill();
            log.Info("System Stop Transmitting");
            if (viewModel is MapViewModel)
            {
                var vm = viewModel as MapViewModel;
                vm.IsTransmitting = false;
            }
            else
            {
                var vmw = viewModel as TransmitInOrderViewModel;
                vmw.IsTransmitting = false;
            }
        }
        /// <summary>
        /// Generate Files By Sequence Number(NumberInOrder) in order to transmit them.
        /// </summary>
        /// <param name="viewModel"></param>
        public void GenerateInOrder(TransmitInOrderViewModel viewModel)
        {
            var list = new List<CoordinateViewModel>();
            foreach (var coordinate in _marker.GetAll())
            {
                if (coordinate.NumberInOrder > 0 && coordinate.HasFile)
                {
                    list.Add(new CoordinateViewModel(coordinate));
                }
            }
            if (list.Count < 1)
            {
               
            }
            else
            {
                var listToGenerate = list.OrderBy(P => P.NumberInOrder)
                                         .Select(p => String.Concat(p.Name.Where(c => !Char.IsWhiteSpace(c))))
                                         .ToList();
                CombineFileToSingleFile(listToGenerate);
                log.Info("Order Generated");
            }
        }
        /// <summary>
        /// Transmit All the Locations that was specefied to the list.
        /// </summary>
        /// <param name="viewModel"></param>
        public void TransmitInOrder(TransmitInOrderViewModel viewModel)
        {
            if (!BaseCommand.PingHost("10.0.0.41"))
            {
                throw new PingException("Not Connected");
            }
            transmit("Streak");
            log.Info("Transmitting in order");
            viewModel.IsTransmitting = true;

        }
        private void transmit(string viewModel)
        {
            proccess.StartInfo.FileName = "tx_samples_from_file.exe";
            proccess.StartInfo.RedirectStandardInput = true;
            proccess.StartInfo.UseShellExecute = false;
            proccess.StartInfo.CreateNoWindow = true;
            proccess.StartInfo.RedirectStandardOutput = false;
            proccess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            proccess.StartInfo.Arguments = $@"--file {String.Concat(viewModel.Where(c => !Char.IsWhiteSpace(c)))}.bin --type short --rate 2500000 --freq 1575420000 --gain 0 --repeat --ref external";
            proccess.Start();
            if (proccess.HasExited)
            {
                throw new SDRException();
            }
            Thread.Sleep(5000);
        }
        public void CombineFileToSingleFile(List<string> list)
        {
            var anotherList = new List<string>();
            foreach (var item in list)
            {
                var file = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory).Where(p => p.Contains(item)).SingleOrDefault();
                anotherList.Add(file);
            }

            using (var outputStream = File.Create(AppDomain.CurrentDomain.BaseDirectory + "/Streak.bin"))
            {
                foreach (var inputFilePath in anotherList)
                {
                    using (var inputStream = File.OpenRead(inputFilePath))
                    {
                        inputStream.CopyTo(outputStream);
                    }
                }
            }
        }
    }
}
