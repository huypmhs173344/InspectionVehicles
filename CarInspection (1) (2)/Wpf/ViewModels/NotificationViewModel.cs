using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BusinessObject.Models;
using Services;
using Wpf.Utils;

namespace Wpf.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {
        private readonly INotificationService _notificationService;
        private ObservableCollection<Notification> _notifications;
        private Notification _selectedNotification;

        public NotificationViewModel()
        {
            _notificationService = new NotificationService();
            LoadNotifications();
            InitializeCommands();
        }

        public ObservableCollection<Notification> Notifications
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
                    (MarkAsReadCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand MarkAsReadCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        private void InitializeCommands()
        {
            MarkAsReadCommand = new RelayCommand(param => ExecuteMarkAsRead(), param => CanExecuteMarkAsRead());
            DeleteCommand = new RelayCommand(param => ExecuteDelete(), param => CanExecuteDelete());
        }

        private void LoadNotifications()
        {
            var notifications = _notificationService.GetNotificationsByUserId(SessionManager.CurrentUser.UserId);
            Notifications = new ObservableCollection<Notification>(notifications);
        }

        private void ExecuteMarkAsRead()
        {
            if (SelectedNotification != null)
            {
                _notificationService.MarkAsRead(SelectedNotification.NotificationId);
                LoadNotifications();
            }
        }

        private bool CanExecuteMarkAsRead()
        {
            return SelectedNotification?.IsRead == false;
        }

        private void ExecuteDelete()
        {
            if (SelectedNotification != null)
            {
                _notificationService.DeleteNotification(SelectedNotification.NotificationId);
                LoadNotifications();
            }
        }

        private bool CanExecuteDelete()
        {
            return SelectedNotification != null;
        }
    }
}
