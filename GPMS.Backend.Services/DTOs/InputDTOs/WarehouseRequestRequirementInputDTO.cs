using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs
{
    public class WarehouseRequestRequirementInputDTO
    {
        public Guid ProducitonRequirementId { get; set; }
        public int Quantity { get; set; }
    }
}
