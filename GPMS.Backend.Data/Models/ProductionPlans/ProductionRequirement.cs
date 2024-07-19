using GPMS.Backend.Data.Enums.Times;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Requests;

namespace GPMS.Backend.Data.Models.ProductionPlans
{
    public class ProductionRequirement
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }

        public Guid ProductSpecificationId { get; set; }
        public ProductSpecification ProductSpecification { get; set; }
        public Guid ProductionPlanId { get; set; }
        public ProductionPlan ProductionPlan { get; set; }

        public ICollection<ProductionEstimation> ProductionEstimations { get; set; } = new List<ProductionEstimation>();
        public ICollection<WarehouseRequest> WarehouseRequests { get; set; } = new List<WarehouseRequest>();
    }
}
