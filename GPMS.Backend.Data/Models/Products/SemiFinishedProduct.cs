using GPMS.Backend.Data.Models.Products.ProductionProcesses;

namespace GPMS.Backend.Data.Models.Products
{
    public class SemiFinishedProduct
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public ICollection<ProductionProcessStepIO> ProductionProcessStepIOs { get; set; } = new List<ProductionProcessStepIO>();
    }
}
