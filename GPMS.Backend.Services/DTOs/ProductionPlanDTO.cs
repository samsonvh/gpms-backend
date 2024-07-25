using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
using GPMS.Backend.Data.Enums.Types;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Staffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class ProductionPlanDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime ExpectedStartingDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ActualStartingDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public ProductionPlanType Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public ProductionPlanStatus Status { get; set; }

        public Guid CreatorId { get; set; }
        public Guid? ReviewerId { get; set; }
        public Guid? ParentProductionPlanId { get; set; }

        public List<ProductionRequirementDTO> ProductionRequirementDTOs { get; set; }
    }
}
