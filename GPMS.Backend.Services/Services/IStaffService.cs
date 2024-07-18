using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services
{
    public interface IStaffService
    {
        Task<IEnumerable<StaffListingDTO>> GetAllStaffs();
        Task<StaffDTO> Details(Guid id, CurrentLoginUserDTO currentLoginUserDTO);
    }
}
