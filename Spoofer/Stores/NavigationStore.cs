using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Stores
{
    public class NavigationStore
    {
        private ViewModelBase _baseViewModel;

        public ViewModelBase BaseViewModel
        {
            get => _baseViewModel;
            set
            {
                _baseViewModel = value;
                OnCurrentViewModelChanged();
            }
        }
        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
        public event Action CurrentViewModelChanged;
    }
}
