using log4net;
using Spoofer.Commands.UserCommands;
using Spoofer.Exceptions;
using Spoofer.Services.Marker;
using Spoofer.Services.User;
using Spoofer.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using MapControl = Microsoft.Toolkit.Wpf.UI.Controls.MapControl;

namespace Spoofer.Commands.MarkersCommand
{
    public class AddMark : BaseCommand
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly MapViewModel _mapViewModel;
        private readonly IMarkerService _service;


        public AddMark(MapViewModel mapViewModel, IMarkerService service)
        {
            _mapViewModel = mapViewModel;
            _service = service;
            _mapViewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            OnCanExecuteChange();

        }
        public override bool CanExecute(object parameter)
        {
            return base.CanExecute(parameter) && !_mapViewModel.IsTransmitting && RoleAdministration.IsInRole("Admin", "SuperUser");
        }
        public override void Execute(object parameter)
        {
            try
            {
                _mapViewModel.ErrorMessageViewModel.Refresh();
                _service.AddOrUpdateMarker(_mapViewModel, true);
                var map = parameter as MapControl;
                onMapUpdated(map, _service);
                log.Info($"New Marker {_mapViewModel.Label} Was Added");
            }
            catch (InvalidCoordinateException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                log.Error(ex.Message);
            }
            catch (CoordinateExistException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error("Unexpected", ex);
            }
        }
    }
}