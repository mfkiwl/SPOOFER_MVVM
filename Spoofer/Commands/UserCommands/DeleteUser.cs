using Spoofer.Services.User.Repository;
using Spoofer.ViewModels;
using Spoofer.ViewModels.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Spoofer.Commands.UserCommands
{
    public class DeleteUser : BaseCommand
    {
        private readonly UserViewModel _userViewModel;
        private readonly IRepository<Models.User> _repository;
        public DeleteUser(IRepository<Models.User> repository, UserViewModel userViewModel)
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

            if (_userViewModel.SelectedUser != null)
            {
                var user = _repository.GetByViewModel(_userViewModel.SelectedUser.Id);
                if (!user.Permission.Contains("SuperUser")) return true;
                else return false;
            }
            else
            {
                return false;
            }
        }
        public override void Execute(object parameter)
        {
            _repository.Remove(_userViewModel.SelectedUser.Id);
            _repository.Save();
            _userViewModel.UpdateData();
        }
    }
}
