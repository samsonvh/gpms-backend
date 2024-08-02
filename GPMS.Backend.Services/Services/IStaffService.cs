using GPMS.Backend.Services.DTOs;
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
    public interface IStaffService
    {
        Task<StaffDTO> Details(Guid id);
        Task<DefaultPageResponseListingDTO<StaffListingDTO>> GetAll(StaffFilterModel staffFilterModel);
    }
}
