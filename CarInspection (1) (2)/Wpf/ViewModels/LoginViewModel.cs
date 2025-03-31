using System.Windows;
using System.Windows.Input;
using Services;
using Wpf.Utils;
using Wpf.Views;

namespace Wpf.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private string _email;
        private string _password = string.Empty;
        private string _errorMessage;
        private bool _isLoading;

        public LoginViewModel()
        {
            _userService = new UserService();
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string Email
        {
            get => _email;
            set
            {
                if (SetProperty(ref _email, value))
                {
                    (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
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
                    (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }

        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
        }

        private void ExecuteLogin(object parameter)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(Email))
                {
                    ErrorMessage = "Vui lòng nhập email";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Vui lòng nhập mật khẩu";
                    return;
                }

                //if (!Email.Contains("@") || !Email.Contains("."))
                //{
                //    ErrorMessage = "Email không hợp lệ";
                //    return;
                //}

                bool isValid = _userService.ValidateUser(Email, Password);
                if (isValid)
                {
                    var user = _userService.GetUserByEmail(Email);
                    SessionManager.CurrentUser = user;

                    MessageBox.Show(
                        "Đăng nhập thành công!",
                        "Thông báo",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                    // Authorize
                    if (user.Role.Equals("Chủ phương tiện"))
                    {                    
                        var mainWindow = new MainWindow(user);
                        mainWindow.Show();
                    }
                    if (user.Role.Equals("Công nhân kiểm tra"))
                    {                                             
                        var mainManagerWindow = new MainManagerWindow(user);
                        mainManagerWindow.Show();
                    }
                    if (user.Role.Equals("Quản trị viên"))
                    {
                        var userView = new UserView();
                        userView.Show();
                    }
                    if (user.Role.Equals("Cảnh sát giao thông"))
                    {
                        var policeView = new MainPoliceScreen();
                        policeView.Show();
                    }




                    // Close login window
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is LoginView)
                        {
                            window.Close();
                            break;
                        }
                    }
                }
                else
                {
                    ErrorMessage = "Email hoặc mật khẩu không đúng";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Đã xảy ra lỗi khi đăng nhập: " + ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
