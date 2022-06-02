using Spoofer.Data;
using Spoofer.Services.Navigation;
using Spoofer.Services.Spoofer;
using Spoofer.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace Spoofer.Services.User
{
    public class ServiceLogin : ILogin
    {
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
                //throw new FileNotExistException("Username Or Password are Incorrect");
                model.ErrorMessageViewModel.ErrorMessage = "Username Or Password are Incorrect";
                model.IsLoading = false;

            }
            else
            {

                var year = DateTime.Now.Year.ToString();
                var dayOfYear = DateTime.Now.DayOfYear - 1;
                string remoteUri = $"https://data.unavco.org/archive/gnss/rinex/nav/{year}/{dayOfYear}/";
                string fileName = $@"ab11{dayOfYear}0.{year.Substring(2)}n.Z", myStringWebResource = null;
                var ephFiles = new DirectoryInfo(Environment.CurrentDirectory).GetFiles().Where(p => p.Name.Contains($".{year.Substring(2)}n")).OrderBy(o => o.LastWriteTime);
                var file = ephFiles.FirstOrDefault();
                if (file == null || file.LastWriteTimeUtc.Date != DateTime.Today)
                {
                    foreach (var filein in ephFiles)
                    {
                        filein.Delete();
                    }
                    myStringWebResource = remoteUri + fileName;
                    WebClient client = new WebClient();
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("oriri123", "Oriri123");
                    client.Headers.Add(HttpRequestHeader.Cookie, "v~de73b5db86e3962ef8c0c585ceda1a8234ccabe8~vpv~1~v11.rlc~1653382430264");
                    client.DownloadFile(myStringWebResource, fileName);
                    var newFile = Path.GetFileNameWithoutExtension(fileName);
                    Process.Start(@"C:\Program Files\WinRAR\Winrar.exe", $@"E -y {Environment.CurrentDirectory}/{fileName}");
                }
                if (file != null && _context.Coordinates != null && file.LastWriteTimeUtc == DateTime.Today)
                {

                    foreach (var coordinate in _context.Coordinates)
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
                _navigation.Navigate();
            }
        }

    }
}

