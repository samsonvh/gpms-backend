using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.LisingDTOs
{
    public class SpecificationListingDTO
    {
        public Guid Id { get; set; }
        public int InventoryQuantity { get; set; }
        public List<string>? ImageURLs { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
    }
}