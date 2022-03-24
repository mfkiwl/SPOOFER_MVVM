using Spoofer.Stores;

namespace Spoofer.ViewModels
{
    public class MainViewModel : ViewModelBase
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