using System.Windows;
using Wpf.ViewModels;

namespace Wpf.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
        }

        private void RegisterLink_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterView();
            registerWindow.Show();
            this.Close();
        }
    }
}
