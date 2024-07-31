using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class WarehouseRequestRequirementDTO
    {
        public Guid ProductionRequirementId { get; set; }
        public int Quantity { get; set; }
    }
}
