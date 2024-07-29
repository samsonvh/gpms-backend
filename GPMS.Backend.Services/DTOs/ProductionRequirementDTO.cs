using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class ProductionRequirementDTO
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }

        public SpecificationDTO ProductSpecification { get; set; }
        public List<ProductionEstimationDTO> ProductionEstimations { get; set; } = new List<ProductionEstimationDTO>();

    }
}
