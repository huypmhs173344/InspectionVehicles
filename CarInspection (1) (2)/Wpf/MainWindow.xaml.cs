﻿using System.Windows;
using BusinessObject.Models;
using Repositories;
using Services;
using Wpf.Utils;
using Wpf.ViewModels;
using Wpf.Views;

namespace Wpf
{
    public partial class MainWindow : Window
    {
        private readonly User _currentUser;

        public MainWindow(User user = null)
        {
            InitializeComponent();
            _currentUser = user;
            DataContext = this;

            // Show home view by default
            var homeView = new HomeView
            {
                DataContext = new HomeViewModel()
            };
            MainContent.Content = homeView;
        }

        public User CurrentUser => _currentUser;

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            var homeView = new HomeView
            {
                DataContext = new HomeViewModel()
            };
            MainContent.Content = homeView;
        }

        private void VehicleManagement_Click(object sender, RoutedEventArgs e)
        {
            var vehicleView = new VehicleView { DataContext = new VehicleViewModel() };
            MainContent.Content = vehicleView;
        }

        private void InspectionRegistration_Click(object sender, RoutedEventArgs e)
        {
            var inspectionView = new InspectionView { DataContext = new InspectionViewModel() };
            MainContent.Content = inspectionView;
        }

        private void InspectionHistory_Click(object sender, RoutedEventArgs e)
        {
            var historyView = new HistoryView
            {
                DataContext = new HistoryViewModel()
            };
            MainContent.Content = historyView;
        }
        private void Notification_Click(object sender, RoutedEventArgs e)
        {
            var notificationView = new NotificationView { DataContext = new NotificationViewModel() };
            MainContent.Content = notificationView;
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
