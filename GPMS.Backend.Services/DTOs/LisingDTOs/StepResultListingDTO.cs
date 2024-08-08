using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.LisingDTOs
{
    public class StepResultListingDTO
    {
        public Guid Id { get; set; }
        public string? InspectionRequestResultCode { get; set; }
        public string StepCode { get; set; }
        public string ProductionSeriesCode { get; set; }
    }
}
