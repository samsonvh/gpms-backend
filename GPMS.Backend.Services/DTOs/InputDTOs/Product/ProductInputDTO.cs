using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using Microsoft.AspNetCore.Http;

namespace GPMS.Backend.Services.DTOs.Product.InputDTOs.Product
{
    public class ProductInputDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Sizes { get; set; }
        public string Colors { get; set; }
        public List<IFormFile> Images { get; set; }

        
        public CategoryInputDTO Category { get; set; }
        public List<SemiFinishedProductInputDTO> SemiFinishedProducts { get; set; }
        public List<SpecificationInputDTO> Specifications { get; set; }
        public List<ProcessInputDTO> Processes { get; set; }

    }
}