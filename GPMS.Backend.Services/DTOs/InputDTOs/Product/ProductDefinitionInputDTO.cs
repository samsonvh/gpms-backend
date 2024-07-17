using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using Microsoft.AspNetCore.Http;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Product
{
    public class ProductDefinitionInputDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Sizes { get; set; }
        public string Colors { get; set; }
        // public List<IFormFile>? Images { get; set; }
        public string ImageURLs { get; set; }
        public string Category { get; set; }
        public List<SemiFinishedProductInputDTO> SemiFinishedProducts { get; set; }
        public List<MaterialInputDTO> Materials { get; set; }

    }
}