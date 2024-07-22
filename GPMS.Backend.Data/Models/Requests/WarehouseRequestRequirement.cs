using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Warehouses;

namespace GPMS.Backend.Data.Models.Requests
{
    public class WarehouseRequestRequirement
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }


        public Guid WarehouseRequestId { get; set; }
        public WarehouseRequest WarehouseRequest { get; set; }
        public Guid ProductionRequirementId { get; set; }
        public ProductionRequirement ProductionRequirement { get; set; }
    }
}