using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spoofer.ViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set 
            { _errorMessage = value;
                OnPropertyChanged(nameof(HasError));
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool HasError => !String.IsNullOrEmpty(ErrorMessage);
        public void Refresh()
        {
            ErrorMessage = String.Empty;
        }

    }
}
