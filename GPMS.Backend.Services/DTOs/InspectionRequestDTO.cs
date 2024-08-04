using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class InspectionRequestDTO
    {
        public Guid Id { get; set; }
        public string ProductionSeriesCode { get; set; }
        public string CreatorName { get; set; }
        public string? ReviewerName { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
