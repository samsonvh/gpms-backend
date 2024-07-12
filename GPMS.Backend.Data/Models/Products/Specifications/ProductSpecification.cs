using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Results;
using GPMS.Backend.Data.Models.Warehouses;

namespace GPMS.Backend.Data.Models.Products.Specifications
{
    public class ProductSpecification
    {
        public Guid Id { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int InventoryQuantity { get; set; }

        public Guid ProductId { get; set; }
        public  Product Product { get; set; }
        public Guid WarehouseId { get; set; }
        public  Warehouse Warehouse { get; set; }

        public ICollection<ProductionRequirement> ProductionRequirements { get; set; } = new List<ProductionRequirement>();
        public ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();
        public ICollection<FaultyProduct> FaultyProducts { get; set; } = new List<FaultyProduct>();
        public ICollection<WarehouseTicket> WarehouseTickets { get; set; } = new List<WarehouseTicket>();
        public ICollection<BillOfMaterial> BillOfMaterials { get; set; } = new List<BillOfMaterial>();
        public ICollection<QualityStandard> QualityStandards {get; set;} = new List<QualityStandard>();
    }
}
