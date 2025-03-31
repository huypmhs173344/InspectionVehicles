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
using System.Windows.Shapes;
using BusinessObject.Models;
using Wpf.Utils;
using Wpf.ViewModels;
using Wpf.Views;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainPoliceScreen.xaml
    /// </summary>
    public partial class MainPoliceScreen : Window
    {
        private readonly User _currentUser;
        public MainPoliceScreen(User user = null)
        {
            InitializeComponent();
            _currentUser = user;
            DataContext = this;
            var vehicleManagementView = new RecordView { DataContext = new InspectionRecordViewModel() };
            MainContent.Content = vehicleManagementView;
        }
        public User CurrentUser => _currentUser;
        private void RecordManagement_Click(object sender, RoutedEventArgs e)
        {
            var recordView = new RecordView { DataContext = new InspectionRecordViewModel() };
            MainContent.Content = recordView;
        }
        private void NotificationManagement_Click(object sender, RoutedEventArgs e)
        {
            var notiView = new PoliceNotification { DataContext = new PoliceNotificationViewModel() };
            MainContent.Content = notiView;
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                SessionManager.ClearSession();
                var loginWindow = new Views.LoginView();
                loginWindow.Show();
                this.Close();
            }
        }
    }
}
