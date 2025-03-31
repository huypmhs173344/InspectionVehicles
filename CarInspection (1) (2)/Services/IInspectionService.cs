using System.Collections.Generic;
using BusinessObject.Models;

namespace Services
{
    public interface IInspectionService
    {
        IEnumerable<InspectionStation> GetAllStations();
        IEnumerable<InspectionRecord> GetAllInspections();
        void CreateInspectionRegistration(InspectionRecord record);
        IEnumerable<InspectionRecord> GetInspectionsByVehicleId(int vehicleId);
        IEnumerable<InspectionRecord> GetInspectionsByVehicleIdWithDetails(int vehicleId);
        InspectionRecord GetInspectionById(int recordId);
        InspectionRecord GetInspectionByIdWithDetails(int recordId);
        void UpdateInspection(InspectionRecord record);
        void DeleteInspection(int recordId);
    }
}
