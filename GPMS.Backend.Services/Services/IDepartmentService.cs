using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentListingDTO>> GetAllDepartments();
        Task<DepartmentDTO> Details(Guid id, CurrentLoginUserDTO currentLoginUserDTO);
    }
}
