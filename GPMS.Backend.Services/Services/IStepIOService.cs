using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;

namespace GPMS.Backend.Services.Services
{
    public interface IStepIOService
    {
        Task AddList(List<StepIOInputDTO> inputDTOs, Guid stepId, 
        List<CreateUpdateResponseDTO<Material>> materialCodeList, 
        List<CreateUpdateResponseDTO<SemiFinishedProduct>> semiFinsihedProductCodeList);
        Task<DefaultPageResponseListingDTO<StepIOListingDTO>> GetAll(StepIOFilterModel stepIOFilterModel);
    }
}