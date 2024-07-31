using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Requests
{
    public class WarehouseRequestRequirementInputDTOWrapper
    {
        public List<WarehouseRequestRequirementInputDTO> WarehouseRequestRequirementInputDTOList { get; set; } = new List<WarehouseRequestRequirementInputDTO>();
    }
}
