using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Data.Models.Products.ProductionProcesses
{
    public class ProductProductionProcess
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public string? Description { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public ICollection<ProductionProcessStep> Steps { get; set; } = new List<ProductionProcessStep>();
    }
}
