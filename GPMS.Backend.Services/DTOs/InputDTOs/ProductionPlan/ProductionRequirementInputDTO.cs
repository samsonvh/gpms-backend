using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan
{
    public class ProductionRequirementInputDTO
    {
        public Guid ProductionSpecificationId { get; set; }
        public int Quantity { get; set; }
        public List<ProductionEstimationInputDTO> ProductionEstimationInputDTOs { get; set; }
    }
}
