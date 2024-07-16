using GPMS.Backend.Data.Enums.Others;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Results;

namespace GPMS.Backend.Data.Models.Staffs
{
    public class Staff
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public StaffPosition Position { get; set; }
        public StaffStatus Status { get; set; }

        public Guid AccountId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Account Account { get; set; }
        public Department? Department { get; set; }

        public ICollection<Product> CreatedProducts { get; set; } = new List<Product>();
        public ICollection<Product> ReviewedProducts { get; set; } = new List<Product>();
        public ICollection<ProductionPlan> CreatedProductionPlans { get; set; } = new List<ProductionPlan>();
        public ICollection<ProductionPlan> ReviewedProductionPlans { get; set; } = new List<ProductionPlan>();
        public ICollection<InspectionRequest> CreatedInspectionRequests { get; set; } = new List<InspectionRequest>();
        public ICollection<InspectionRequest> ReviewedInspectionRequests { get; set; } = new List<InspectionRequest>();
        public ICollection<WarehouseRequest> CreatedWarehouseRequests { get; set; } = new List<WarehouseRequest>();
        public ICollection<WarehouseRequest> ReviewedWarehouseRequests { get; set; } = new List<WarehouseRequest>();
        public ICollection<InspectionRequestResult> InspectionRequestResults { get; set; } = new List<InspectionRequestResult>();
        public ICollection<ProductionProcessStepResult> productionProcessStepResults { get; set; } = new List<ProductionProcessStepResult>();
    }
}
