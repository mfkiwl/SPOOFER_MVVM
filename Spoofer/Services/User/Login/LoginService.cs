using log4net;
using Spoofer.Data;
using Spoofer.Services.Marker;
using Spoofer.Services.Navigation;
using Spoofer.Services.Spoofer;
using Spoofer.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;

namespace Spoofer.Services.User
{
    public class ServiceLogin : ILogin
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CoordinatesContext _context;
        private readonly NavigationService _navigation;
        private readonly ISpooferService _spoofer;

        public ServiceLogin(CoordinatesContext context, NavigationService navigation, ISpooferService spoofer)
        {
            _context = context;
            _navigation = navigation;
            _spoofer = spoofer;
        }

        public void OnLogin(AccountViewModel model)
        {
            model.ErrorMessageViewModel.ErrorMessage = "";
            model.IsLoading = true;

            if (!_context.User.Any(p => p.UserName == model.UserName && p.Password == model.Password))
            {
                log.Debug("False");
                model.ErrorMessageViewModel.ErrorMessage = "Username Or Password are Incorrect";
                model.IsLoading = false;
                log.Error("Error");
            }
            else
            {
                log.Debug("True");
                var year = DateTime.Now.Year.ToString();
                var dayOfYear = DateTime.Now.DayOfYear - 1;
                string remoteUri = $"https://data.unavco.org/archive/gnss/rinex/nav/{year}/{dayOfYear}/";
                string fileName = $@"ab11{dayOfYear}0.{year.Substring(2)}n.Z", myStringWebResource = null;
                log.Debug("Details");
                var ephFiles = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles().Where(p => p.Name.Contains($".{year.Substring(2)}n")).OrderBy(o => o.LastWriteTime);
                log.Debug(ephFiles);
                var file = ephFiles.FirstOrDefault();
                log.Debug(file);
                if (file == null || file.LastWriteTimeUtc.Date != DateTime.Today)
                {
                    log.Debug("DetectFiles");
                    foreach (var filein in ephFiles)
                    {
                        filein.Delete();
                    }
                    log.Debug("Not updated file deleted Seccesfully");
                    myStringWebResource = remoteUri + fileName;
                    log.Debug(myStringWebResource);
                    WebClient client = new WebClient();
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("oriri123", "Oriri123");
                    log.Debug(client);
                    client.Headers.Add(HttpRequestHeader.Cookie, "v~de73b5db86e3962ef8c0c585ceda1a8234ccabe8~vpv~1~v11.rlc~1653382430264");
                    client.DownloadFile(myStringWebResource, fileName);
                    log.Debug("Ephemeris File Created Succesfully");
                    var newFile = Path.GetFileNameWithoutExtension(fileName);
                    Process.Start(@"C:\Program Files\WinRAR\Winrar.exe", $@"E -y {AppDomain.CurrentDomain.BaseDirectory}/{fileName}");
                    log.Debug("Extracted Succesfully");
                }
                Thread.Sleep(300);
                if (_context.Coordinates != null)
                {

                    foreach (var coordinate in _context.Coordinates)
                    {
                        if (coordinate.GenerationDate != DateTime.Today && coordinate.GenerationDate != null)
                        {
                            var flags = new string[11];
                            flags[0] = $"Core.dll";
                            flags[1] = "-e";
                            flags[2] = $"{Path.GetFileNameWithoutExtension(fileName.Trim())}";
                            flags[3] = "-s";
                            flags[4] = "2500000";
                            flags[5] = "-l";
                            flags[6] = $"{coordinate.Latitude.ToString().Trim()},{coordinate.Longitude.ToString().Trim()},{coordinate.Height.ToString().Trim()}";
                            flags[7] = "-o";
                            flags[8] = $"{String.Concat(coordinate.Name.Where(c => !Char.IsWhiteSpace(c)))}.bin";
                            flags[9] = "-d";
                            flags[10] = "65";
                            var argc = flags.Length;
                            _spoofer.GenerateIQFile(flags);
                        }
                    }
                }
                log.Debug("All Files Is Up To Date");
                _navigation.Navigate();
            }
        }

    }
}

