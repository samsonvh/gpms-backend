using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification
{
    public class ImageSpecificationInputDTO
    {
        public Guid SpecificationId { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}