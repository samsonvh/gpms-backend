using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Requests
{
    public class InspectionRequestInputDTO
    {
        public Guid ProductionSeriesId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
    }
}
