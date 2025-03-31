using System.Windows;
using Wpf.ViewModels;

namespace Wpf.Views
{
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
            DataContext = new RegisterViewModel();
        }

        private void LoginLink_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginView();
            loginWindow.Show();
            this.Close();
        }
    }
}
