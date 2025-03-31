using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.Data.SqlClient;
using Services;
using System.Windows.Input;
using Wpf.Utils;
using Dapper;

namespace Wpf.ViewModels
{
    public class PoliceNotificationViewModel : ViewModelBase
    {
        private readonly INotificationService _notificationService;
        private ObservableCollection<Notification> _notifications;
        private Notification _selectedNotification;
        private string _searchText;
        private bool _isEditing;
        private string _messageError;
        private string _dateError;
        private string _userError;

        public string MessageError
        {
            get => _messageError;
            set => SetProperty(ref _messageError, value);
        }

        public string DateError
        {
            get => _dateError;
            set => SetProperty(ref _dateError, value);
        }
        public string UserError
        {
            get => _userError;
            set => SetProperty(ref _userError, value);
        }
        private void ClearErrors()
        {
            MessageError = string.Empty;
            DateError = string.Empty;
        }

        private bool ValidateRecord()
        {
            ClearErrors();
            bool isValid = true;

            // Kiểm tra tin nhắn không được để trống hoặc null
            if (string.IsNullOrWhiteSpace(SelectedNotification?.Message))
            {
                MessageError = "Tin nhắn không được để trống";
                isValid = false;
            }

            // Kiểm tra ngày gửi
            if (!SelectedNotification?.SentDate.HasValue ?? true) // Kiểm tra null trước khi truy cập HasValue
            {
                DateError = "Ngày gửi không được để trống";
                isValid = false;
            }
            else if (SelectedNotification.SentDate > DateTime.Now)
            {
                DateError = "Ngày gửi không được là ngày trong tương lai";
                isValid = false;
            }
            if (!IsValidUser(SelectedNotification.UserId))
            {
                UserError = "Không có người này";
                isValid = false;
            }

            return isValid;
        }
        private bool IsValidUser(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(1) FROM Users WHERE UserID = @UserId";
                return connection.ExecuteScalar<int>(query, new { UserId = userId }) > 0;
            }
        }
        public PoliceNotificationViewModel()
        {
            _notificationService = new NotificationService();
            LoadNotification();
            InitializeCommands();
        }
        public ObservableCollection<Notification> notifications
        {
            get => _notifications;
            set => SetProperty(ref _notifications, value);
        }
        public Notification SelectedNotification
        {
            get => _selectedNotification;
            set
            {
                if (SetProperty(ref _selectedNotification, value))
                {
                    (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    FilterNotification();
                }
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (SetProperty(ref _isEditing, value))
                {
                    (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand LoadData { get; private set; }

        private void InitializeCommands()
        {
            AddCommand = new RelayCommand(param => ExecuteAdd());
            EditCommand = new RelayCommand(param => ExecuteEdit(), param => CanExecuteEdit());
            SaveCommand = new RelayCommand(param => ExecuteSave());
            CancelCommand = new RelayCommand(param => ExecuteCancel());
        }

        private void LoadNotification()
        {
            var noti = GetNotificationDetail();
            notifications = new ObservableCollection<Notification>(noti);
        }

        private void FilterNotification()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadNotification();
                return;
            }

            var filteredNotification = GetNotificationDetail()
                .Where(n =>
                    (n.User?.FullName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
                    || (n.UserId.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                )
                .ToList();

            notifications = new ObservableCollection<Notification>(filteredNotification);
        }

        private void ExecuteAdd()
        {
            SelectedNotification = new Notification();
            IsEditing = true;
        }

        private void ExecuteEdit()
        {
            if (SelectedNotification != null)
            {
                IsEditing = true;
            }
        }

        private bool CanExecuteEdit()
        {
            return SelectedNotification != null && !IsEditing;
        }
        private void ExecuteSave()
        {
            if (!ValidateRecord())
            {
                return;
            }

            if (SelectedNotification.NotificationId == 0)
            {
                _notificationService.CreateNotification(SelectedNotification);
            }
            else
            {
                _notificationService.UpdateNotification(SelectedNotification);
            }

            IsEditing = false;
            LoadNotification();
        }

        private void ExecuteCancel()
        {
            IsEditing = false;
            SelectedNotification = null;
        }

        //new
        private readonly string _connectionString = "Server=MHuy;uid=sa;password=123456;database=CarInspectionDB;Encrypt=True;TrustServerCertificate=True;";
        public List<Notification> GetNotificationDetail()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT 
                n.NotificationID,
                n.IsRead,
                n.Message,
                n.SentDate,
                n.UserID,
                u.FullName
            FROM Notifications n
            JOIN dbo.[Users] u ON n.UserID = u.UserID";

                var noti = connection.Query<NotificationDTO>(query).Select(n => new Notification
                {
                    NotificationId = n.NotificationId,
                    IsRead = n.IsRead,
                    Message = n.Message,
                    SentDate = n.SentDate,
                    UserId = n.UserId,
                    User = new User
                    {
                        UserId = n.UserId,
                        FullName = n.FullName
                    },
                }).ToList();

                return noti;
            }
        }
    }
}
