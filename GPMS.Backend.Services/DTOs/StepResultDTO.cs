using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class StepResultDTO
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        
        public Guid CreatorId { get; set; }
        public Guid ProductionSeriesId { get; set; }
        public Guid? InspectionRequestResultId { get; set; }
        
    }
}