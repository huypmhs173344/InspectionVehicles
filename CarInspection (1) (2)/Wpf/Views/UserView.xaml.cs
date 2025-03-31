using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BusinessObject.Models;
using Services;
using Wpf.Utils;

namespace Wpf.Views
{
    public partial class UserView : Window
    {
        private readonly IUserService _userService;
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<string> AvailableRoles { get; set; }
        public User SelectedUser { get; set; }
        public string SearchTerm { get; set; }

        public ICommand SearchUserCommand { get; }
        public ICommand SaveChangesCommand { get; }

        public UserView()
        {
            InitializeComponent();
            _userService = new UserService();
            Users = new ObservableCollection<User>(_userService.GetAllUsers());

            AvailableRoles = new ObservableCollection<string>
            {
                "Chủ phương tiện",
                "Công nhân kiểm tra",
                "Quản trị viên",
                "Cảnh sát giao thông"
            };

            SearchUserCommand = new RelayCommand(_ => SearchUser());
            SaveChangesCommand = new RelayCommand(_ => SaveChanges());

            DataContext = this;
        }

        private void SearchUser()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchTerm))
                {
                    Users = new ObservableCollection<User>(_userService.GetAllUsers());
                }
                else
                {
                    var userList = _userService.SearchUsers(SearchTerm);
                    Users.Clear();
                    foreach (var user in userList)
                    {
                        Users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm người dùng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveChanges()
        {
            try
            {
                if (SelectedUser != null)
                {
                    _userService.UpdateUser(SelectedUser);
                    MessageBox.Show("Cập nhật thông tin người dùng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật người dùng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
