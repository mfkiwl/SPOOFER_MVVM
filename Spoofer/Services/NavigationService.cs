using log4net;
using Spoofer.Stores;
using Spoofer.ViewModels;
using System;

namespace Spoofer.Services.Navigation
{
    public class NavigationService
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly NavigationStore _navigationStore;
        private readonly Func<ViewModelBase> _createViewModel;
        public NavigationService()
        {

        }
        public NavigationService(NavigationStore navigationStore, Func<ViewModelBase> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }
        public void Navigate()
        {
            _navigationStore.BaseViewModel = _createViewModel();
            log.Info($"System Navigated to {_navigationStore.BaseViewModel}");
        }
    }
}