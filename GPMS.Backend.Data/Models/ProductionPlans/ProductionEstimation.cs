using GPMS.Backend.Data.Enums.Times;

namespace GPMS.Backend.Data.Models.ProductionPlans
{
    public class ProductionEstimation
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public int OvertimeQuantity { get; set; }
        public Quarter? Quarter { get; set; }
        public Month? Month { get; set; }
        public int? Batch {  get; set; }
        public int? DayNumber {  get; set; }

        public Guid ProductionRequirementId { get; set; }
        public ProductionRequirement ProductionRequirement { get; set; }

        public ICollection<ProductionSeries> ProductionSeries { get; set; } = new List<ProductionSeries>();
    }
}
