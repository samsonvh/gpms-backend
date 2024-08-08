using GPMS.Backend.Services.DTOs.InputDTOs.Results;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services
{
    public interface IIOStepResultService
    {
        Task<DefaultPageResponseListingDTO<IOResultListingDTO>> GetAllIOResultByStepResult(Guid stepResultId, IOResultFilterModel resultFilterModel);
        Task AddList(Guid stepId,Guid stepResultId,List<InputOutputResultInputDTO> inputDTOs);
    }
}
