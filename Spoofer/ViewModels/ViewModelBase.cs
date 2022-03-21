﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.ViewModels
{
      public class ViewModelBase : INotifyPropertyChanged
      {
          public event PropertyChangedEventHandler PropertyChanged;
          protected void OnPropertyChanged(string propertyName)
          {
              PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
          }
      }
}
