using Spoofer.Stores;
using Spoofer.ViewModels;
using System;
using System.Diagnostics;

namespace Spoofer.Services.Navigation
{
    public class NavigationService
    {
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
            var processInfo = new ProcessStartInfo("docker", $"exec -it c8c905025ba8e7aefe0fb2e9213044eec6ca9289d12f3928c972adef13f7216a /bin/sh");
            processInfo.UseShellExecute = false;
            var proccess = new Process();
            proccess.StartInfo = processInfo;
            proccess.Start();
        }
    }
}