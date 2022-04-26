using log4net;
using Spoofer.Commands.UserCommands;
using Spoofer.Data;
using Spoofer.Exceptions;
using Spoofer.EXMethods;
using Spoofer.Services.Marker;
using Spoofer.Services.Spoofer;
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
        private readonly ISpoofer _spoofer;
        private readonly MapViewModel _mapViewModel;
        public Generate(IMarkerService marker, ISpoofer spoofer, MapViewModel mapViewModel)
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
            catch(CoordinateNotExistException ex)
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
            catch (Exception e)
            {
                log.Error("Spoofing File not Generated For a reason", e);
                MessageBox.Show(e.Message);
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
                flags[4] = "2500000";
                flags[5] = "-l";
                flags[6] = $"{_mapViewModel.Latitude},{_mapViewModel.Longitude},{_mapViewModel.Height}";
                flags[7] = "-o";
                flags[8] = $"{_mapViewModel.Label.Trim()}.bin";
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

