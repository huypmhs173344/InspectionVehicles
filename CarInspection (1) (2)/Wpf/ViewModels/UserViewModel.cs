using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BusinessObject.Models;
using Services;
using Wpf.Utils;
using Wpf.ViewModels;

namespace ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        public User SelectedUser { get; set; }
        public string SearchTerm { get; set; }
        public ICommand SearchUserCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }

        public UserViewModel(IUserService userService)
        {
            _userService = userService;
            LoadUsers();

            SearchUserCommand = new RelayCommand(_ => SearchUser());
            SaveChangesCommand = new RelayCommand(_ => SaveChanges(), _ => SelectedUser != null);
            DeleteUserCommand = new RelayCommand(_ => DeleteUser(), _ => SelectedUser != null);
        }

        private void LoadUsers()
        {
            try
            {
                Users = new ObservableCollection<User>(_userService.GetAllUsers());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách người dùng: {ex.Message}");
            }
        }

        private void SearchUser()
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                Users = new ObservableCollection<User>(_userService.SearchUsers(SearchTerm));
            }
        }

        private void SaveChanges()
        {
            if (SelectedUser != null)
            {
                _userService.UpdateUser(SelectedUser);
                MessageBox.Show("Cập nhật thành công!");
            }
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                _userService.DeleteUser(SelectedUser.UserId);
                Users.Remove(SelectedUser);
                MessageBox.Show("Xóa thành công!");
            }
        }
    }
}
