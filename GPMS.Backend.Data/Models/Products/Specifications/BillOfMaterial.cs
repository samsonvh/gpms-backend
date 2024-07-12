namespace GPMS.Backend.Data.Models.Products.Specifications
{
    public class BillOfMaterial
    {
        public Guid Id { get; set; }
        public float SizeWidth { get; set; }
        public float Consumption { get; set; }
        public string? Description { get; set; }

        public Guid MaterialId { get; set; }
        public Material Material { get; set; }
        public Guid ProductSpecificationId { get; set; }
        public ProductSpecification ProductSpecification { get; set; }
    }
}
