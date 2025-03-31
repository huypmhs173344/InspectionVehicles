using System.Collections.Generic;
using System.Linq;
using BusinessObject.Models;
using Repositories;

namespace Services
{
    public class InspectionRecordService : IInspectionRecordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InspectionRecordService()
        {
            _unitOfWork = new UnitOfWork(new CarInspectionDbContext());
        }
        

        public void CreateInspectionRecord(InspectionRecord record)
        {
            _unitOfWork.InspectionRecordRepository.Add(record);
            _unitOfWork.Save();
        }

        public void DeleteInspectionRecord(int id)
        {
            var record = _unitOfWork.InspectionRecordRepository.GetById(id);
            if (record != null)
            {
                _unitOfWork.InspectionRecordRepository.Delete(record);
                _unitOfWork.Save();
            }
        }

        public IEnumerable<InspectionRecord> GetAllInspectionRecords()
        {
            return _unitOfWork.InspectionRecordRepository.GetAll();
        }

        public InspectionRecord GetInspectionRecordById(int id)
        {
            return _unitOfWork.InspectionRecordRepository.GetById(id);
        }

        public IEnumerable<InspectionRecord> GetInspectionRecordsByInspectorId(int inspectorId)
        {
            return _unitOfWork
                .InspectionRecordRepository.GetAll()
                .Where(r => r.InspectorId == inspectorId);
        }

        public IEnumerable<InspectionRecord> GetInspectionRecordsByStationId(int stationId)
        {
            return _unitOfWork
                .InspectionRecordRepository.GetAll()
                .Where(r => r.StationId == stationId);
        }

        public IEnumerable<InspectionRecord> GetInspectionRecordsByVehicleId(int vehicleId)
        {
            return _unitOfWork
                .InspectionRecordRepository.GetAll()
                .Where(r => r.VehicleId == vehicleId);
        }

        public void UpdateInspectionRecord(InspectionRecord record)
        {
            _unitOfWork.InspectionRecordRepository.Update(record);
            _unitOfWork.Save();
        }
    }
}
