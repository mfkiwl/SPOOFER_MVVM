﻿using log4net;
using Microsoft.Toolkit.Wpf.UI.Controls;
using Spoofer.Commands.UserCommands;
using Spoofer.Exceptions;
using Spoofer.Services.Marker;
using Spoofer.Services.User;
using Spoofer.ViewModels;
using System;
using System.ComponentModel;
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
            return base.CanExecute(parameter) && !_mapViewModel.IsTransmitting && RoleAdministration.IsInRole("Admin", "SuperUser");
        }
        public override void Execute(object parameter)
        {
            try
            {
                _mapViewModel.ErrorMessageViewModel.Refresh();
                _service.RemoveMarker(_mapViewModel, false);
                var map = parameter as MapControl;
                OnMapUpdated(map, _service);
                log.Info($"{_mapViewModel.Label} Removed Succesfully");
            }
            catch (CoordinateNotExistException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                log.Error(ex.Message);
            }
            catch (InvalidCoordinateException ex)
            {
                _mapViewModel.ErrorMessageViewModel.ErrorMessage = ex.Message;
                log.Error(ex.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                log.Error("Unexpected", e);
            }
        }
    }
}