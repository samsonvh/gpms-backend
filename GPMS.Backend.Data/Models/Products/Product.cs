using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Staffs;

namespace GPMS.Backend.Data.Models.Products
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Sizes { get; set; }
        public string Colors { get; set; }
        public string ImageURLs { get; set; }
        public DateTime CreatedDate { get; set; }
        public ProductStatus Status { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid CreatorId { get; set; }
        public Staff Creator { get; set; }
        public Guid? ReviewerId { get; set; }
        public Staff? Reviewer { get; set; }

        public ICollection<ProductSpecification> Specifications { get; set; } = new List<ProductSpecification>();
        public ICollection<SemiFinishedProduct> SemiFinishedProducts { get; set; } = new List<SemiFinishedProduct>();
        public ICollection<ProductProductionProcess> ProductionProcesses { get; set; } = new List<ProductProductionProcess>();
    }
}
