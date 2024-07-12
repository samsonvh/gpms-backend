using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Results;

namespace GPMS.Backend.Data.Models.ProductionPlans
{
    public class ProductionSeries
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Quantity { get; set; }
        public int? FaultyQuantity { get; set; }
        public string? CurrentProcess { get; set; }
        public ProductionSeriesStatus Status { get; set; }

        public Guid ProductionEstimationId { get; set; }
        public ProductionEstimation ProductionEstimation { get; set; }

        public ICollection<InspectionRequest> InspectionRequests { get; set; } = new List<InspectionRequest>();
        public ICollection<ProductionProcessStepResult> ProductionProcessStepResults { get; set; }
    }
}
