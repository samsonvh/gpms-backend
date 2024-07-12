using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Products.Specifications;

namespace GPMS.Backend.Data.Models.Products
{
    public class Material
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ConsumptionUnit { get; set; }
        public string SizeWidthUnit { get; set; }
        public string ColorCode { get; set; }
        public string ColorName { get; set; }
        public string Description { get; set; }

        public ICollection<BillOfMaterial> BillOfMaterials { get; set; } = new List<BillOfMaterial>();
        public ICollection<QualityStandard> QualityStandards { get; set; } = new List<QualityStandard>();
        public ICollection<ProductionProcessStepIO> ProcessStepIOs { get; set; } = new List<ProductionProcessStepIO>();
    }
}
