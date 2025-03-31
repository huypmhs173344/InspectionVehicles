using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;
using Repositories;

namespace Services
{
    public class StationService : IStationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StationService()
        {
            _unitOfWork = new UnitOfWork(new CarInspectionDbContext());
        }

        public void CreateStation(InspectionStation station)
        {
            var maxId = _unitOfWork.InspectionStationRepository.GetAll().Max(u => u.StationId);
            station.StationId = maxId + 1;
            _unitOfWork.InspectionStationRepository.Add(station);
            _unitOfWork.Save();
        }

        public void DeleteStation(int id)
        {
            var station = _unitOfWork.InspectionStationRepository.GetById(id);
            if (station != null)
            {
                _unitOfWork.InspectionStationRepository.Delete(station);
                _unitOfWork.Save();
            }
        }

        public IEnumerable<InspectionStation> GetAllStation()
        {
            return _unitOfWork.InspectionStationRepository.GetAll();
        }
        public void UpdateStation(InspectionStation station)
        {
            _unitOfWork.InspectionStationRepository.Update(station);
            _unitOfWork.Save();
        }
    }
}
