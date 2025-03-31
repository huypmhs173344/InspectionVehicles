using System;
using BusinessObject.Models;

namespace Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CarInspectionDbContext _context;
        private IRepository<User> _userRepository;
        private IRepository<Vehicle> _vehicleRepository;
        private IRepository<InspectionRecord> _inspectionRecordRepository;
        private IRepository<InspectionStation> _inspectionStationRepository;
        private IRepository<Log> _logRepository;
        private IRepository<Notification> _notificationRepository;

        public UnitOfWork(CarInspectionDbContext context)
        {
            _context = context;
        }

        public IRepository<User> UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new GenericRepository<User>(_context);
                }
                return _userRepository;
            }
        }

        public IRepository<Vehicle> VehicleRepository
        {
            get
            {
                if (_vehicleRepository == null)
                {
                    _vehicleRepository = new GenericRepository<Vehicle>(_context);
                }
                return _vehicleRepository;
            }
        }

        public IRepository<InspectionRecord> InspectionRecordRepository
        {
            get
            {
                if (_inspectionRecordRepository == null)
                {
                    _inspectionRecordRepository = new GenericRepository<InspectionRecord>(_context);
                }
                return _inspectionRecordRepository;
            }
        }

        public IRepository<InspectionStation> InspectionStationRepository
        {
            get
            {
                if (_inspectionStationRepository == null)
                {
                    _inspectionStationRepository = new GenericRepository<InspectionStation>(
                        _context
                    );
                }
                return _inspectionStationRepository;
            }
        }

        public IRepository<Log> LogRepository
        {
            get
            {
                if (_logRepository == null)
                {
                    _logRepository = new GenericRepository<Log>(_context);
                }
                return _logRepository;
            }
        }

        public IRepository<Notification> NotificationRepository
        {
            get
            {
                if (_notificationRepository == null)
                {
                    _notificationRepository = new GenericRepository<Notification>(_context);
                }
                return _notificationRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
