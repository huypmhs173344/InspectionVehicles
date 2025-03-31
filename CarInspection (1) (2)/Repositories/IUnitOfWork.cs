using System;

namespace Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<BusinessObject.Models.User> UserRepository { get; }
        IRepository<BusinessObject.Models.Vehicle> VehicleRepository { get; }
        IRepository<BusinessObject.Models.InspectionRecord> InspectionRecordRepository { get; }
        IRepository<BusinessObject.Models.InspectionStation> InspectionStationRepository { get; }
        IRepository<BusinessObject.Models.Log> LogRepository { get; }
        IRepository<BusinessObject.Models.Notification> NotificationRepository { get; }
        void Save();
    }
}
