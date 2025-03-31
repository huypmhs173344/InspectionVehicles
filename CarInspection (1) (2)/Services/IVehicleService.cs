using BusinessObject.Models;
using System.Collections.Generic;

namespace Services
{
    public interface IVehicleService
    {
        IEnumerable<Vehicle> GetAllVehicles();
        IEnumerable<Vehicle> GetAllVehiclesWithInspections();
        Vehicle GetVehicleById(int id);
        Vehicle GetVehicleByIdWithInspections(int id);
        IEnumerable<Vehicle> GetVehiclesByOwnerId(int ownerId);
        IEnumerable<Vehicle> GetVehiclesByOwnerIdWithInspections(int ownerId);
        void CreateVehicle(Vehicle vehicle);
        void UpdateVehicle(Vehicle vehicle);
        void DeleteVehicle(int id);
    }
}
