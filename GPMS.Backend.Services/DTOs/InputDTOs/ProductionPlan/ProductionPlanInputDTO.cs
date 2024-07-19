using GPMS.Backend.Data.Enums.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan
{
    public class ProductionPlanInputDTO
    {
        public Guid? ParentProductionPlanId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime ExpectedStartingDate { get; set; }
        public DateTime DueDate { get; set; }
        public ProductionPlanType Type { get; set; }
        public List<ProductionRequirementInputDTO> ProductionRequirementInputDTOs { get; set; }
    }
}
