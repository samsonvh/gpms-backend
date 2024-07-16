using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
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
        Task<ChangeStatusResponseDTO<Staff, StaffStatus>> ChangeStatus(Guid id, StaffStatus newStatus);
    }
}
