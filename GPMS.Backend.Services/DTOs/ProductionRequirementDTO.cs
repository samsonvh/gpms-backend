using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class ProductionRequirementDTO
    {
        public Guid ProductionSpecificationId { get; set; }
        public Guid ProductionPlanId { get; set; }
        public int Quantity { get; set; }
        public SpecificationDTO Specification { get; set; } 
    }
}
