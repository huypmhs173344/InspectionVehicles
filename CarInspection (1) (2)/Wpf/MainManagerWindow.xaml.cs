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
    /// Interaction logic for MainManagerWindow.xaml
    /// </summary>
    public partial class MainManagerWindow : Window
    {
        private readonly User _currentUser;
        public MainManagerWindow(User user = null)
        {
            InitializeComponent();
            _currentUser = user;
            DataContext = this;
            var vehicleManagementView = new VehicleManagementView { DataContext = new VehicleManagementViewModel() };
            MainContent.Content = vehicleManagementView;
        }
        public User CurrentUser => _currentUser;

        private void VehicleManagement_Click(object sender, RoutedEventArgs e)
        {
            var vehicleManagementView = new VehicleManagementView { DataContext = new VehicleManagementViewModel() };
            MainContent.Content = vehicleManagementView;
        }

        private void StationManagement_Click(object sender, RoutedEventArgs e)
        {
            var stationManagementView = new StationView { DataContext = new StationViewModel() };
            MainContent.Content = stationManagementView;
        }
        private void RecordManagement_Click(object sender, RoutedEventArgs e)
        {
            var recordView = new InspectionRecordView { DataContext = new InspectionRecordViewModel() };
            MainContent.Content = recordView;
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
