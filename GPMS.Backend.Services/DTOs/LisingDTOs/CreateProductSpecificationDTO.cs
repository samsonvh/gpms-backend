using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class CreateProductSpecificationListingDTO
    {
        public Guid Id { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
    }
}