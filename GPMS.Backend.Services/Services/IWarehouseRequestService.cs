using GPMS.Backend.Data.Enums.Statuses.Requests;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Requests;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services
{
    public interface IWarehouseRequestService
    {
        Task<WarehouseRequestDTO> Details(Guid id);
        Task<CreateUpdateResponseDTO<WarehouseRequest>> Add(WarehouseRequestInputDTO inputDTO, CurrentLoginUserDTO currentLoginUserDTO);
        Task<ChangeStatusResponseDTO<WarehouseRequest, WarehouseRequestStatus>> ChangeStatus(Guid id, ChangeStatusInputDTO inputDTO);
    }
}
