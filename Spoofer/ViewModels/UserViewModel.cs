using Spoofer.Commands.MarkersCommands;
using Spoofer.Commands.UserCommands;
using Spoofer.Models;
using Spoofer.Services.Marker;
using Spoofer.Services.User.Repository;
using Spoofer.ViewModels.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Spoofer.ViewModels
{
    public class UserViewModel : ViewModelBase    
    {
        private readonly IMarkerService _markerToMap;
        private readonly IMarkerService _markerToSequence;
        private ObservableCollection<UserDTOViewModel> users;
        private readonly IRepository<User> _admin;
        public ICollectionView Users { get; }
        public UserViewModel(IRepository<User> admin, IMarkerService markerToMap, IMarkerService markerToSequence)
        {
            _admin = admin;
            _markerToMap = markerToMap;
            _markerToSequence = markerToSequence;
            users = new ObservableCollection<UserDTOViewModel>();
            Users = CollectionViewSource.GetDefaultView(UpdateData());
            UserFormViewModel = new UserFormViewModel();
            ErrorMessageViewModel = new MessageViewModel();
            Delete = new DeleteUser(admin, this);
            NavigateToMap = new Navigate(_markerToMap, this);
            NavigateToSequence = new Navigate(_markerToSequence, this);
            AddUser = new AddUser(admin, this);
        }
        public ICommand NavigateToMap { get;}
        public ICommand NavigateToSequence { get;}
        public ICommand AddUser { get;  }
        public ICommand Delete { get;}
        private UserDTOViewModel _selectedUser;

        public UserDTOViewModel SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; OnPropertyChanged(nameof(SelectedUser)); }
        }
        public UserFormViewModel UserFormViewModel { get; set; }
        private string _selectedPermission;

        public string SelectedPermission
        {
            get { return _selectedPermission; }
            set { _selectedPermission = value; OnPropertyChanged(nameof(SelectedPermission)); }
        }

        public MessageViewModel ErrorMessageViewModel { get;  set; }

        public ObservableCollection<UserDTOViewModel> UpdateData()
        {
            users.Clear();
            var list = _admin.GetAll();
            foreach (var user in list)
            {
                var viewModel = new UserDTOViewModel(user);
                users.Add(viewModel);
            }
            return users;
        }
    }
}
