using log4net;
using Spoofer.Data;
using Spoofer.Services.Marker;
using Spoofer.Services.Navigation;
using Spoofer.Services.Spoofer;
using Spoofer.Services.User.Repository;
using Spoofer.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using Spoofer.Models;
using Spoofer.Commands.UserCommands;
using System.Windows;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Spoofer.Services.User
{
    public class ServiceLogin : ILogin
    {
        private BackgroundWorker loginWorker;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly NavigationService _navigation;
        private readonly IRepository<Models.User> _userRepo;
        private readonly IRepository<Models.Coordinates> _coordinatesRepo;
        private readonly ISpooferService _spoofer;

        public ServiceLogin(IRepository<Models.User> userRepo, IRepository<Coordinates> coordinateRepo, NavigationService navigation, ISpooferService spoofer)
        {
            _userRepo = userRepo;
            _navigation = navigation;
            _coordinatesRepo = coordinateRepo;
            _spoofer = spoofer;
            loginWorker = new BackgroundWorker();
            loginWorker.DoWork += LoginWorker_DoWork;
            loginWorker.ProgressChanged += LoginWorker_ProgressChanged;
            loginWorker.RunWorkerCompleted += LoginWorker_RunWorkerCompleted;
            loginWorker.WorkerReportsProgress = true;
        }

        private void LoginWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoginWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoginWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Login the user, download the updated ephemeris file, and Update all the saved spoofing files 
        /// </summary>
        /// <param name="model"></param>
        public async void OnLogin(LoginViewModel model)
        {
            model.ErrorMessageViewModel.ErrorMessage = "";
            model.IsLoading = true;
            if (!_userRepo.GetAll().Any())
            {
                var user = new Models.User()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    Permission = "SuperUser",
                    Password = model.Password
                };
                _userRepo.AddOrUpdate(user);
                _userRepo.Save();
            }
            if (!_userRepo.GetAll().Any(p => p.UserName == model.UserName && p.Password == model.Password))
            {
                log.Debug("False");
                model.ErrorMessageViewModel.ErrorMessage = "Username Or Password are Incorrect";
                model.IsLoading = false;
                log.Error("Error");
            }
            else
            {
                var year = DateTime.Now.Year.ToString();
                var dayOfYear = DateTime.Now.DayOfYear - 1;
                string dayOfYearString = dayOfYear.ToString();
                if (dayOfYear < 10)
                {
                    dayOfYearString = $"0{dayOfYear}";
                }
                else if (dayOfYear < 100)
                {
                    dayOfYearString = $"0{dayOfYear}";
                }
                string remoteUri = $"https://data.unavco.org/archive/gnss/rinex/nav/{year}/{dayOfYearString}/";
                string fileName = $@"ab14{dayOfYearString}0.{year.Substring(2)}n.Z", myStringWebResource = null;
                var ephFiles = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles().Where(p => p.Name.Contains($".{year.Substring(2)}n")).OrderBy(o => o.LastWriteTime);
                log.Debug(ephFiles);
                var file = ephFiles.FirstOrDefault();
                log.Debug(file);
                var proccess = new Process();
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
                    using (HttpClient client = new HttpClient())
                    {

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Im80WDNMM1p0QkN6MmZ5RktMVW9mWiJ9.eyJpc3MiOiJodHRwczovL2xvZ2luLmVhcnRoc2NvcGUub3JnLyIsInN1YiI6Imdvb2dsZS1vYXV0aDJ8MTEwMTI4ODEwNjc1NTIyMDMzMTg2IiwiYXVkIjpbImh0dHBzOi8vYWNjb3VudC5lYXJ0aHNjb3BlLm9yZyIsImh0dHBzOi8vZWFydGhzY29wZS1wcm9kLnVzLmF1dGgwLmNvbS91c2VyaW5mbyJdLCJpYXQiOjE2ODA1MDA5OTQsImV4cCI6MTY4MDUyOTc5NCwiYXpwIjoiak14UWJiS1BFc3pXWDBhOTNBR2xCWlo4cm45MTdvY3giLCJzY29wZSI6Im9wZW5pZCBwcm9maWxlIGVtYWlsIiwicGVybWlzc2lvbnMiOltdfQ.Q9oOG0E3OrTWVF2BdRci4chcj4SuP20rzI-6sya_UbhAzNI6Dr17jwmH9ajpPmn_QfZpq7hlSOlHrAbFPTENjaxLIS0wRiCqnkwnj-9nA9zneVPTs5a0G-CrPtyYGZ0gWUHj8gIxtwKHf_I2D-bFTWfoo-2wp9D1Yfj5L-7Gs9Jg6JIYdQJUqNcJzS1D3F8ldF0rXceianNKBruFqHMBx6nBnJMzdo7Ebv86_AdPEGRLicLYdC9aKqcRIb-2O7kIEfR9VUv8Jn1APzZX4nC-qe5fcjgAi2pQTd3vL4QEGanwTHndq5GUyRjB4YAryrd9FRQjYNnEYx0pqN1rgZ_CTQ");
                        HttpResponseMessage response = await client.GetAsync(myStringWebResource);
                        response.EnsureSuccessStatusCode();
                        byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                        File.WriteAllBytes(fileName, fileBytes);
                    }
                    proccess.StartInfo.FileName = "7z.exe";
                    proccess.StartInfo.RedirectStandardInput = true;
                    proccess.StartInfo.UseShellExecute = false;
                    proccess.StartInfo.CreateNoWindow = false;
                    proccess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    proccess.StartInfo.Arguments = $@"x {fileName}";
                    log.Debug(proccess.StartInfo.Arguments);
                    proccess.Start();
                }
                Thread.Sleep(300);
                log.Info($@"Arguments: {proccess.StartInfo.Arguments}");
                log.Debug("Extracted Successfully");
                var newFile = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles().Where(p => p.Name.Contains($".{year.Substring(2)}n")).OrderBy(o => o.LastWriteTime).FirstOrDefault();
                if (_coordinatesRepo.GetAll() != null)
                {
                    foreach (var coordinate in _coordinatesRepo.GetAll())
                    {
                        if (newFile.LastWriteTime.AddMinutes(1) >= DateTime.Now)
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



                var user = _userRepo.GetAll().SingleOrDefault(p => p.UserName == model.UserName && p.Password == model.Password);
                var authUser = user;
                authUser.IsAuthenticated = true;
                _userRepo.Update(authUser);
                _userRepo.Save();
                log.Debug("All Files Is Up To Date");
                _navigation.Navigate();

            }
        }
    }

}


