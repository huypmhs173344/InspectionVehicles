using System.Collections.Generic;
using System.Linq;
using BusinessObject.Models;
using Repositories;

namespace Services
{
    public class InspectionService : IInspectionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InspectionService()
        {
            _unitOfWork = new UnitOfWork(new CarInspectionDbContext());
        }

        public IEnumerable<InspectionRecord> GetAllInspections()
        {
            return _unitOfWork.InspectionRecordRepository.GetAll();
        }

        public IEnumerable<InspectionStation> GetAllStations()
        {
            return _unitOfWork.InspectionStationRepository.GetAll();
        }

        public void CreateInspectionRegistration(InspectionRecord record)
        {
            var maxRecordId = _unitOfWork.InspectionRecordRepository.GetAll().Max(r => r.RecordId);
            var newRecord = new InspectionRecord
            {
                RecordId = maxRecordId + 1,
                VehicleId = record.VehicleId,
                StationId = record.StationId,
                InspectionDate = record.InspectionDate,
            };
            _unitOfWork.InspectionRecordRepository.Add(newRecord);
            _unitOfWork.Save();
        }

        public IEnumerable<InspectionRecord> GetInspectionsByVehicleId(int vehicleId)
        {
            return _unitOfWork
                .InspectionRecordRepository.GetAll()
                .Where(r => r.VehicleId == vehicleId);
        }

        public IEnumerable<InspectionRecord> GetInspectionsByVehicleIdWithDetails(int vehicleId)
        {
            return _unitOfWork
                .InspectionRecordRepository.GetAllIncluding(
                    new[] { "Vehicle", "Inspector", "Station" }
                )
                .Where(r => r.VehicleId == vehicleId);
        }

        public InspectionRecord GetInspectionById(int recordId)
        {
            return _unitOfWork.InspectionRecordRepository.GetById(recordId);
        }

        public InspectionRecord GetInspectionByIdWithDetails(int recordId)
        {
            return _unitOfWork.InspectionRecordRepository.GetByIdIncluding(
                recordId,
                new[] { "Vehicle", "Inspector", "Station" }
            );
        }

        public void UpdateInspection(InspectionRecord record)
        {
            _unitOfWork.InspectionRecordRepository.Update(record);
            _unitOfWork.Save();
        }

        public void DeleteInspection(int recordId)
        {
            var record = GetInspectionById(recordId);
            if (record != null)
            {
                _unitOfWork.InspectionRecordRepository.Delete(record);
                _unitOfWork.Save();
            }
        }
    }
}
