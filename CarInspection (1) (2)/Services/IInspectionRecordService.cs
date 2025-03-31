using BusinessObject.Models;
using System.Collections.Generic;

namespace Services
{
    public interface IInspectionRecordService
    {
        IEnumerable<InspectionRecord> GetAllInspectionRecords();
        InspectionRecord GetInspectionRecordById(int id);
        IEnumerable<InspectionRecord> GetInspectionRecordsByVehicleId(int vehicleId);
        IEnumerable<InspectionRecord> GetInspectionRecordsByStationId(int stationId);
        IEnumerable<InspectionRecord> GetInspectionRecordsByInspectorId(int inspectorId);
        void CreateInspectionRecord(InspectionRecord record);
        void UpdateInspectionRecord(InspectionRecord record);
        void DeleteInspectionRecord(int id);

    }
}
