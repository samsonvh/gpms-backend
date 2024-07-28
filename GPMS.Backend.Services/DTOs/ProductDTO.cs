using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Enums.Statuses.Products;

namespace GPMS.Backend.Services.DTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Sizes { get; set; }
        public string Colors { get; set; }
        public string ImageURLs { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }
}