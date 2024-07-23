using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;

namespace GPMS.Backend.Services.DTOs.Product.InputDTOs.Product
{
    public class ProductInputDTO
    {
        public ProductDefinitionInputDTO Definition { get; set; }
        public List<SpecificationInputDTO> Specifications { get; set; }
        public List<ProcessInputDTO> Processes { get; set; }

    }
}