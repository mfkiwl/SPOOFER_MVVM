using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Spoofer.Views
{
    /// <summary>
    /// Interaction logic for AccountView.xaml
    /// </summary>
    public partial class AccountView : UserControl
    {
        public AccountView()
        {
            InitializeComponent();
        }

        private void goRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            goLoginButton.Visibility = Visibility.Visible;
            goRegisterButton.Visibility = Visibility.Collapsed;
            loginButton.Visibility = Visibility.Collapsed;
            registerButton.Visibility = Visibility.Visible;
            loginCheck.Visibility = Visibility.Collapsed;
            LoginCheckBox.Visibility = Visibility.Collapsed;
        }
        private void goLoginButton_Click(object sender, RoutedEventArgs e)
        {
            goLoginButton.Visibility = Visibility.Collapsed;
            goRegisterButton.Visibility = Visibility.Visible;
            loginButton.Visibility = Visibility.Visible;
            registerButton.Visibility = Visibility.Collapsed;
            loginCheck.Visibility = Visibility.Visible;
            LoginCheckBox.Visibility = Visibility.Visible;
        }
    }
}
