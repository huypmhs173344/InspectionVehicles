using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using BusinessObject.Models;
using Services;
using Wpf.Utils;
using Wpf.Views;

namespace Wpf.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        public IUserService UserService => _userService;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _fullName = string.Empty;
        private string _phone = string.Empty;
        private string _address = string.Empty;
        private string _errorMessage = string.Empty;
        private string _selectedRole = "Chủ phương tiện";
        private bool _isLoading;
        private List<string> _roles;

        public List<string> Roles
        {
            get => _roles;
            set => SetProperty(ref _roles, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (SetProperty(ref _selectedRole, value))
                {
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public RegisterViewModel()
        {
            _userService = new UserService();
            RegisterCommand = new RelayCommand(ExecuteRegister, CanExecuteRegister);
            Roles = new List<string>
            {
                "Chủ phương tiện",
                "Công nhân kiểm tra",
                "Quản trị viên",
                "Cảnh sát giao thông",
            };
        }

        public string Email
        {
            get => _email;
            set
            {
                if (SetProperty(ref _email, value))
                {
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                {
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (SetProperty(ref _confirmPassword, value))
                {
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string FullName
        {
            get => _fullName;
            set
            {
                if (SetProperty(ref _fullName, value))
                {
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand RegisterCommand { get; }

        private bool CanExecuteRegister(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Email)
                && !string.IsNullOrWhiteSpace(Password)
                && !string.IsNullOrWhiteSpace(ConfirmPassword)
                && !string.IsNullOrWhiteSpace(FullName)
                && Password == ConfirmPassword;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Vui lòng nhập email";
                return false;
            }

            if (!Email.Contains("@") || !Email.Contains("."))
            {
                ErrorMessage = "Email không hợp lệ";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Vui lòng nhập mật khẩu";
                return false;
            }

            if (Password.Length < 6)
            {
                ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự";
                return false;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Mật khẩu xác nhận không khớp";
                return false;
            }

            if (string.IsNullOrWhiteSpace(FullName))
            {
                ErrorMessage = "Vui lòng nhập họ tên";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Phone))
            {
                ErrorMessage = "Vui lòng nhập số điện thoại";
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(Phone, @"^[0-9]{10}$"))
            {
                ErrorMessage = "Số điện thoại không hợp lệ";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                ErrorMessage = "Vui lòng nhập địa chỉ";
                return false;
            }

            if (_userService.IsEmailExists(Email))
            {
                ErrorMessage = "Email đã tồn tại trong hệ thống";
                return false;
            }

            return true;
        }

        private void ExecuteRegister(object parameter)
        {
            IsLoading = true;
            try
            {
                ErrorMessage = string.Empty;

                if (!ValidateInput())
                {
                    IsLoading = false;
                    return;
                }

                var user = new User
                {
                    Email = Email,
                    Password = Password,
                    FullName = FullName,
                    Phone = Phone,
                    Address = Address,
                    Role = SelectedRole switch
                    {
                        "Chủ phương tiện" => "Chủ phương tiện",
                        "Công nhân kiểm tra" => "Công nhân kiểm tra",
                        "Quản trị viên" => "Quản trị viên",
                        "Cảnh sát giao thông" => "Cảnh sát giao thông",
                        _ => "Owner",
                    },
                };

                _userService.CreateUser(user);

                MessageBox.Show(
                    "Đăng ký tài khoản thành công!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                var loginWindow = new LoginView();
                loginWindow.Show();

                foreach (Window window in Application.Current.Windows)
                {
                    if (window is RegisterView)
                    {
                        window.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Đã xảy ra lỗi khi đăng ký: " + ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
