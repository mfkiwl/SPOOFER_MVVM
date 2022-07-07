using Spoofer.Models;
using Spoofer.Services.User.Admin;
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

namespace Spoofer.ViewModels
{
    public class UserViewModel : ViewModelBase    
    {
        private ObservableCollection<UserDTOViewModel> users;
        private readonly IRepository<User> _admin;
        public ICollectionView Users { get; }
        public UserViewModel(IRepository<User> admin)
        {
            _admin = admin;
            users = new ObservableCollection<UserDTOViewModel>();
            Users = CollectionViewSource.GetDefaultView(UpdateData());
        }

        private ObservableCollection<UserDTOViewModel> UpdateData()
        {
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
