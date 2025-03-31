using System.Collections.Generic;
using System.Linq;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService()
        {
            _unitOfWork = new UnitOfWork(new CarInspectionDbContext());
        }

        public IEnumerable<Notification> GetAllNotifications()
        {
            return _unitOfWork.NotificationRepository.GetAll();
        }

        public void MarkAsRead(int notificationId)
        {
            var notification = _unitOfWork.NotificationRepository.GetById(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                _unitOfWork.NotificationRepository.Update(notification);
                _unitOfWork.Save();
            }
        }

        public void DeleteNotification(int notificationId)
        {
            var notification = _unitOfWork.NotificationRepository.GetById(notificationId);
            if (notification != null)
            {
                _unitOfWork.NotificationRepository.Delete(notification);
                _unitOfWork.Save();
            }
        }
        
        public IEnumerable<Notification> GetNotificationsByUserId(int userId)
        {
            return _unitOfWork.NotificationRepository.GetAll().Where(v => v.UserId == userId);
        }

        public void CreateNotification(Notification notification)
        {
            var maxId = _unitOfWork.NotificationRepository.GetAll().Max(n => n.NotificationId);
            notification.NotificationId = maxId + 1;
            _unitOfWork.NotificationRepository.Add(notification);
            _unitOfWork.Save();
        }

        public void UpdateNotification(Notification notification)
        {
            _unitOfWork.NotificationRepository.Update(notification);
            _unitOfWork.Save();
        }
    }
}
