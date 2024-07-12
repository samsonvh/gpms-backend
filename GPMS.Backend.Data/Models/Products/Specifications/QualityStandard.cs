using GPMS.Backend.Data.Models.Results;

namespace GPMS.Backend.Data.Models.Products.Specifications
{
    public class QualityStandard
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }

        public Guid ProductSpecificationId { get; set; }
        public ProductSpecification ProductSpecification { get; set; }
        public Guid? MaterialId { get; set; }
        public Material? Material { get; set; }

        public ICollection<ProductFault> ProductFaults { get; set; } = new List<ProductFault>();
    }
}
