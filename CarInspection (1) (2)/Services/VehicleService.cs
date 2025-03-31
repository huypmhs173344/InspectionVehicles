using System.Collections.Generic;
using System.Linq;
using BusinessObject.Models;
using Repositories;

namespace Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleService()
        {
            _unitOfWork = new UnitOfWork(new CarInspectionDbContext());
        }

        public void CreateVehicle(Vehicle vehicle)
        {
            var maxId = _unitOfWork.VehicleRepository.GetAll().Max(v => v.VehicleId);
            vehicle.VehicleId = maxId + 1;
            _unitOfWork.VehicleRepository.Add(vehicle);
            _unitOfWork.Save();
        }

        public void DeleteVehicle(int id)
        {
            var vehicle = _unitOfWork.VehicleRepository.GetById(id);
            if (vehicle != null)
            {
                _unitOfWork.VehicleRepository.Delete(vehicle);
                _unitOfWork.Save();
            }
        }

        public IEnumerable<Vehicle> GetAllVehicles()
        {
            return _unitOfWork.VehicleRepository.GetAll();
        }

        public IEnumerable<Vehicle> GetAllVehiclesWithInspections()
        {
            return _unitOfWork.VehicleRepository.GetAllIncluding(new[] { "InspectionRecords" });
        }

        public Vehicle GetVehicleById(int id)
        {
            return _unitOfWork.VehicleRepository.GetById(id);
        }

        public Vehicle GetVehicleByIdWithInspections(int id)
        {
            return _unitOfWork.VehicleRepository.GetByIdIncluding(
                id,
                new[] { "InspectionRecords" }
            );
        }

        public IEnumerable<Vehicle> GetVehiclesByOwnerId(int ownerId)
        {
            return _unitOfWork.VehicleRepository.GetAll().Where(v => v.OwnerId == ownerId);
        }

        public IEnumerable<Vehicle> GetVehiclesByOwnerIdWithInspections(int ownerId)
        {
            return _unitOfWork
                .VehicleRepository.GetAllIncluding(new[] { "InspectionRecords" })
                .Where(v => v.OwnerId == ownerId);
        }
        public void UpdateVehicle(Vehicle vehicle)
        {
            _unitOfWork.VehicleRepository.Update(vehicle);
            _unitOfWork.Save();
        }
    }
}
