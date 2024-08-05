using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Services.PageRequests;

namespace GPMS.Backend.Services.DTOs.LisingDTOs
{
    public class ProductListingDTO 
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<string>? ImageURLs { get; set; } = new List<string>();
        public List<string> Sizes { get; set; } = new List<string>();
        public List<string> Colors { get; set; } = new List<string>();
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }
}