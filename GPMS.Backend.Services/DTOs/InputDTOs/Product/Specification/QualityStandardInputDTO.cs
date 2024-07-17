using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification
{
    public class QualityStandardInputDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        // public List<IFormFile>? Images { get; set; }
        public string ImageURLs { get; set; }
        public string MaterialCode { get; set; }
    }
}