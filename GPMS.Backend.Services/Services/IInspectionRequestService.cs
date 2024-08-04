using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services
{
    public interface IInspectionRequestService
    {
        Task<InspectionRequestDTO> Add(InspectionRequestInputDTO inputDTO);
        Task<InspectionRequestDTO> Details(Guid id);
    }
}
