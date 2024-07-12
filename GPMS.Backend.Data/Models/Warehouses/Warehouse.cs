using GPMS.Backend.Data.Models.Products.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Data.Models.Warehouses
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<ProductSpecification> Specifications { get; set; } = new List<ProductSpecification>();
        public ICollection<WarehouseTicket> WarehouseTickets { get; set;} = new List<WarehouseTicket>();
    }
}
