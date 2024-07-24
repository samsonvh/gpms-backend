using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Staffs;
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
        Task<CreateUpdateResponseDTO<WarehouseRequest>> Add(WarehouseRequestInputDTO inputDTO);
    }
}
