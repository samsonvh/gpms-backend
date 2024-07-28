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
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }

        public string CreatorName { get; set; }
        public string? ReviewerName { get; set; }
        public ParentProductionPlanDTO? ParentProductionPlan { get; set; }
        public List<ChildProductionPlanDTO>? ChildProductionPlans { get; set; } = new List<ChildProductionPlanDTO>();

        public List<ProductionRequirementDTO> ProductionRequirements { get; set; }
    }
}
