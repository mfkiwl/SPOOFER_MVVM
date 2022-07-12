using Spoofer.Commands.UserCommands;
using Spoofer.Services.Marker;
using Spoofer.ViewModels;

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
            else if (viewModelBase is TransmitInOrderViewModel)
            {
                var vmw = viewModelBase as TransmitInOrderViewModel;
                return base.CanExecute(parameter) && !vmw.IsTransmitting;
            }
            return true;
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
