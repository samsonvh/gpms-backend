using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using Microsoft.AspNetCore.Http;

namespace GPMS.Backend.Services.DTOs.Product.InputDTOs.Product
{
    public class ProductInputDTO
    {
        public ProductDefinitionInputDTO Definition { get; set; }
        public List<SpecificationInputDTO> Specifications { get; set; }
        public List<ProcessInputDTO> Processes { get; set; }

    }
}