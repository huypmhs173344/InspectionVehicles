using System.Collections.Generic;
using BusinessObject.Models;

namespace Services
{
    public interface INotificationService
    {
        IEnumerable<Notification> GetAllNotifications();
        void MarkAsRead(int notificationId);
        void DeleteNotification(int notificationId);
        IEnumerable<Notification> GetNotificationsByUserId(int userId);
        public void CreateNotification(Notification notification);
        public void UpdateNotification(Notification notification);
    }
}
