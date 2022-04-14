using log4net;
using Spoofer.Commands.UserCommands;
using Spoofer.Data;
using Spoofer.EXMethods;
using Spoofer.Services.Marker;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spoofer.Commands.Spoofing
{
    public class Generate : BaseCommand
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int Counter = 0;
        private readonly IMarkerService _marker;
        private readonly MapViewModel _mapViewModel;
        private int argc;
        public Generate(IMarkerService marker, MapViewModel mapViewModel)
        {
            _marker = marker;
            _mapViewModel = mapViewModel;
            _mapViewModel.PropertyChanged += ViewModelPropertyChanged;
        }
        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChange();
        }
       
        public async override void Execute(object parameter)
        {
            _mapViewModel.ErrorMessageViewModel.Refresh();
            _mapViewModel.IsLoading = true;
            _mapViewModel.IsFinishLoading = false;
            try
            {
                await Task.Run(() => generateFile());
                _mapViewModel.IsLoading = false;
                _mapViewModel.IsFinishLoading = true;
                log.Info("Spoofing File Generated");
            }
            catch (Exception e)
            {
                log.Error("Spoofing File not Generated For a reason", e);
            }
        }
        private string[] GenerateFlags()
        {
            while (true)
            {
                var flags = new string[9];
                flags[0] = "Core.dll";
                flags[1] = "-e";
                flags[2] = "brdc3540.14n";
                flags[3] = "-s";
                flags[4] = "2600000";
                flags[5] = "-l";
                flags[6] = $"{_mapViewModel.Latitude},{_mapViewModel.Longitude},{_mapViewModel.Height}";
                flags[7] = "-o";
                flags[8] = $"{_mapViewModel.Label}.bin";
                return flags;
            }
        }
        public static bool isFileExist(MapViewModel mapViewModel)
        {
            var path = $@"C:\Users\max\source\repos\Spoofer\Spoofer\bin\Debug\{mapViewModel.Label}.bin";
            return File.Exists(path);
        }
        private void generateFile()
        {
            if (!_marker.isExist(_mapViewModel))
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = "There is not valid coordinate specefied for this action";
            }
            else if (isFileExist(_mapViewModel))
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = "File have been created for this marker already";
            }
            else
            {
                argc = GenerateFlags().Length;
                var argv = GenerateFlags();
                if (Counter % 2 == 1)
                {
                    SpoofingMethods.main(argc, argv);
                }
                else
                {
                    SpoofingMethods2.main(argc, argv);
                }
            }
        }


    }
}
