using GPMS.Backend.Data.Models.Products.ProductionProcesses;

namespace GPMS.Backend.Data.Models.Results
{
    public class ProductionProcessStepIOResult
    {
        public Guid Id { get; set; }
        public float? Consumption { get; set; }
        public int? Quantity { get; set; }

        public Guid StepResultId { get; set; }
        public ProductionProcessStepResult ProductionProcessStepResult { get; set; }
        public Guid StepIOId { get; set; }
        public ProductionProcessStepIO ProductionProcessStepIO { get; set; }
    }
}
