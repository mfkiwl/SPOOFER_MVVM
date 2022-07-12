using Spoofer.Services.Marker;
using Spoofer.Services.Navigation;
using Spoofer.Services.User;
using Spoofer.Services.User.Repository;
using Spoofer.Stores;
using Spoofer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Commands.UserCommands
{
    public class NavigateToManagement : BaseCommand
    {
        private readonly IRepository<Models.User> _repository;
        private readonly IMarkerService _marker;
        public NavigateToManagement(IMarkerService marker, IRepository<Models.User> repository)
        {
            _repository = repository;
            _marker = marker;
        }
        public override bool CanExecute(object parameter)
        {
            return RoleAdministration.IsInRole("SuperUser");
        }
        public override void Execute(object parameter)
        {
            _marker.Navigate();
        }
    }
}
