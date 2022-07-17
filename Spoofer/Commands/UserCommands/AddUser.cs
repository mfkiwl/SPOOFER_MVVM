using Spoofer.Services.User.Repository;
using Spoofer.ViewModels;
using Spoofer.ViewModels.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Commands.UserCommands
{
    public class AddUser : BaseCommand
    {
        private readonly IRepository<Models.User> _repository;
        private readonly UserViewModel _userViewModel;
        public AddUser(IRepository<Models.User> repository, UserViewModel userViewModel)
        {
            _repository = repository;
            _userViewModel = userViewModel;
            _userViewModel.PropertyChanged += _userViewModel_PropertyChanged;
        }

        private void _userViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnCanExecuteChange();
        }

        public override bool CanExecute(object parameter)
        {
            return !String.IsNullOrEmpty(_userViewModel.SelectedPermission) &&
                !String.IsNullOrEmpty(_userViewModel.UserFormViewModel.Username) &&
                !String.IsNullOrEmpty(_userViewModel.UserFormViewModel.Password);
        }
        public override void Execute(object parameter)
        {
            var user = new Models.User()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = _userViewModel.UserFormViewModel.Username,
                Password = _userViewModel.UserFormViewModel.Password,
                Permission = _userViewModel.SelectedPermission
            };
            if (!_repository.GetAll().Any(p => p.Password == user.Password || p.UserName == user.UserName))
            {
                _repository.AddOrUpdate(user);
                _repository.Save();
                _userViewModel.UpdateData();
            }
            else
            {
                _userViewModel.ErrorMessageViewModel.ErrorMessage = "This User Already Exist";
            }
        }
    }
}
