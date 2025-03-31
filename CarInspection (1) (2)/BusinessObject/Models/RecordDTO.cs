using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class RecordDTO
    {
        public int RecordId { get; set; }

        public int VehicleId { get; set; }

        public int StationId { get; set; }

        public int? InspectorId { get; set; }

        public DateTime? InspectionDate { get; set; }

        public string? Result { get; set; }

        public decimal? Co2emission { get; set; }

        public decimal? Hcemission { get; set; }

        public string? Comments { get; set; }

        public string Name { get; set; } = null!;

        public string PlateNumber { get; set; } = null!;
        public int? OwnerId { get; set; }
    }
}