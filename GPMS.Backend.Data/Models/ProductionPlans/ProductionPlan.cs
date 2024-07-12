using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
using GPMS.Backend.Data.Enums.Types;
using GPMS.Backend.Data.Models.Staffs;

namespace GPMS.Backend.Data.Models.ProductionPlans
{
    public class ProductionPlan
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
        public Staff Creator { get; set; }
        public Guid? ReviewerId { get; set; }
        public Staff? Reviewer { get; set; }
        public Guid? ParentProductionPlanId { get; set; }
        public ProductionPlan? ParentProductionPlan { get; set; }

        public ICollection<ProductionPlan> ChildProductionPlans { get; set; } = new List<ProductionPlan>();
        public ICollection<ProductionRequirement> ProductionRequirements { get; set; } = new List<ProductionRequirement>();
    }
}
