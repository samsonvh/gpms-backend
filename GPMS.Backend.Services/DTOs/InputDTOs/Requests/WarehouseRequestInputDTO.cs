using GPMS.Backend.Data.Models.ProductionPlans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Requests
{
    public class WarehouseRequestInputDTO
    {
        public Guid CreatorId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<WarehouseRequestRequirementInputDTO> WarehouseRequestRequirementInputDTOs { get; set; }
    }
}
