using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification
{
    public class QualityStandardImagesTemp
    {
        public Guid QualityStandardId { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}