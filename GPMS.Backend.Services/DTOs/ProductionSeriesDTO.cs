using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
using GPMS.Backend.Data.Models.ProductionPlans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class ProductionSeriesDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Quantity { get; set; }
        public int? FaultyQuantity { get; set; }
        public string? CurrentProcess { get; set; }
        public string Status { get; set; }
    }
}
