using Spoofer.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        private readonly NavigationStore _navigation;
        public ViewModelBase CurrentViewModel => _navigation.BaseViewModel;
        public MainViewModel(NavigationStore navigationStore)
        {
            _navigation = navigationStore;
            _navigation.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
