using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.LisingDTOs
{
    public class ProductionPlanListingDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime ExpectedStartingDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
