using Spoofer.Commands.UserCommands;
using Spoofer.Services.Marker;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Commands.MarkersCommands
{
    public class Navigate : BaseCommand
    {
        
        private readonly IMarkerService _service;
        private readonly ViewModelBase viewModelBase;

        public override bool CanExecute(object parameter)
        {
            if (viewModelBase is MapViewModel)
            {
                var vm = viewModelBase as MapViewModel;
                return base.CanExecute(parameter) && !vm.IsTransmitting && !vm.IsLoading;
            }
            var vmw = viewModelBase as TransmitInOrderViewModel;
            return base.CanExecute(parameter) && !vmw.IsTransmitting;
        }
        public Navigate(IMarkerService service, ViewModelBase baseVM)
        {
            _service = service;
            viewModelBase = baseVM;
            
        }
        public override void Execute(object parameter)
        {
            _service.Navigate();
        }
    }
}
