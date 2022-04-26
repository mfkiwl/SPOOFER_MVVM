using log4net;
using Microsoft.Toolkit.Wpf.UI.Controls;
using Spoofer.Commands.UserCommands;
using Spoofer.Exceptions;
using Spoofer.Services.Marker;
using Spoofer.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Spoofer.Commands.MarkersCommand
{
    public class RemoveMark : BaseCommand
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMarkerService _service;
        private readonly MapViewModel _mapViewModel;

        public RemoveMark(MapViewModel mapViewModel, IMarkerService service)
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
            return base.CanExecute(parameter);

        }

        public override void Execute(object parameter)
        {
            try
            {
                _service.RemoveMarker(_mapViewModel);
                var map = parameter as MapControl;
                onMapUpdated(map, _service);
                log.Info("Makrer Removed Succesfully");
            }
            catch(CoordinateNotExistException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
            }
            catch(InvalidCoordinateException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}