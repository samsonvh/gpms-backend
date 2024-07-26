using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Enums.Statuses.Products;

namespace GPMS.Backend.Services.PageRequests
{
    public class ProductPageRequest : DefaultPageRequest
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? Status { get; set; }
    }
}