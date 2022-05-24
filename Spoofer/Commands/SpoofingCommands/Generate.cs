using log4net;
using Spoofer.Commands.UserCommands;
using Spoofer.Exceptions;
using Spoofer.Services.Marker;
using Spoofer.Services.Spoofer;
using Spoofer.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spoofer.Commands.Spoofing
{
    public class Generate : BaseCommand
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int Counter = 0;
        private readonly IMarkerService _marker;
        private readonly ISpooferService _spoofer;
        private readonly MapViewModel _mapViewModel;
        public Generate(IMarkerService marker, ISpooferService spoofer, MapViewModel mapViewModel)
        {
            _marker = marker;
            _spoofer = spoofer;
            _mapViewModel = mapViewModel;
            _mapViewModel.PropertyChanged += ViewModelPropertyChanged;
        }
        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChange();
        }
        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter) && !_mapViewModel.IsTransmitting;
        }

        public async override void Execute(object parameter)
        {

            _mapViewModel.ErrorMessageViewModel.Refresh();
            _mapViewModel.IsLoading = true;
            _mapViewModel.IsFinishLoading = false;
            try
            {
                await Task.Run(() => generateFile());
                Counter++;
                _mapViewModel.IsLoading = false;
                _mapViewModel.IsFinishLoading = true;
                _marker.AddOrUpdateMarker(_mapViewModel, true);
                log.Info("Spoofing File Generated");
            }
            catch (CoordinateNotExistException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                _mapViewModel.IsLoading = false;
                _mapViewModel.IsFinishLoading = true;
            }
            catch (FileAlreadyExistException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                _mapViewModel.IsLoading = false;
                _mapViewModel.IsFinishLoading = true;
            }
            catch (ArgumentException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                _mapViewModel.IsLoading = false;
                _mapViewModel.IsFinishLoading = true;
            }
            catch (Exception e)
            {
                log.Error("Spoofing File not Generated For a reason", e);
                MessageBox.Show(e.Message);
                _mapViewModel.IsLoading = false;
                _mapViewModel.IsFinishLoading = true;
            }
        }
        private string[] GenerateFlags()
        {
            var year = DateTime.Now.Year.ToString();
            var dayOfYear = DateTime.Now.DayOfYear - 1;
            var ephFiles = new DirectoryInfo(Environment.CurrentDirectory).GetFiles().Where(p => p.Name.Contains($".{year.Substring(2)}n")).OrderBy(o => o.LastWriteTime);
            var file = ephFiles.FirstOrDefault();
            foreach (var filein in ephFiles.Skip(1))
            {
                filein.Delete();
            }
            if (file.LastWriteTimeUtc.Date != DateTime.Today || file == null)
            {
                string remoteUri = $"https://data.unavco.org/archive/gnss/rinex/nav/{year}/{dayOfYear}/";
                string fileName = $@"ab01{dayOfYear}0.{year.Substring(2)}n.Z", myStringWebResource = null;
                myStringWebResource = remoteUri + fileName;
                WebClient client = new WebClient();
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("oriri123", "Oriri123");
                client.Headers.Add(HttpRequestHeader.Cookie, "I Dont Know How to get the cookie");
                client.DownloadFile(myStringWebResource, fileName);
                Process.Start(@"C:\Program Files\WinRAR\Winrar.exe", $@"E -y {Environment.CurrentDirectory}/{fileName}");
            }
            while (true)
            {
                var flags = new string[11];
                flags[0] = $"Core.dll";
                flags[1] = "-e";
                flags[2] = $"{file.ToString().Trim()}";
                flags[3] = "-s";
                flags[4] = "2500000";
                flags[5] = "-l";
                flags[6] = $"{_mapViewModel.Latitude.ToString().Trim()},{_mapViewModel.Longitude.ToString().Trim()},{_mapViewModel.Height.ToString().Trim()}";
                flags[7] = "-o";
                flags[8] = $"{String.Concat(_mapViewModel.Label.Where(c => !Char.IsWhiteSpace(c)))}.bin";
                flags[9] = "-d";
                flags[10] = "70";
                return flags;
            }
        }

        private void generateFile()
        {
            var argv = GenerateFlags();
            _spoofer.GenerateIQFile(argv, _mapViewModel);
        }
    }


}

