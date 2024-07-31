using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Requests;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services
{
    public interface IWarehouseRequestRequirementService
    {
        Task<List<CreateUpdateResponseDTO<WarehouseRequestRequirement>>> AddListWarehouseRequestRequirement(List<WarehouseRequestRequirementInputDTO> inputDTOs, Guid warehouseRequestId);
    }
}
