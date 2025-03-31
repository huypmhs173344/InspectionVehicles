using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Models;

namespace Services
{
    public interface IStationService
    {
        IEnumerable<InspectionStation> GetAllStation();
        void CreateStation(InspectionStation station);
        void UpdateStation(InspectionStation station);
        void DeleteStation(int id);
    }
}
