using AutoMapper;
using GPMS.Backend.Data.Enums.Others;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class StaffService : IStaffService
    {
        private readonly IGenericRepository<Staff> _staffRepository;
        private readonly IMapper _mapper;

        public StaffService(IGenericRepository<Staff> staffRepository, IMapper mapper)
        {
            _staffRepository = staffRepository;
            _mapper = mapper;
        }

        public async Task<StaffDTO> Details(Guid id, CurrentLoginUserDTO currentLoginUserDTO)
        {
            var staff = await _staffRepository
                .Search(staff => staff.Id == id)
                .Include(staff => staff.Department)
                .FirstOrDefaultAsync();

            if (staff == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Staff not found");
            }

            if (currentLoginUserDTO.Position == StaffPosition.FactoryDirector.ToString())
            {
                throw new APIException((int)HttpStatusCode.Forbidden, "Factory directors are not allowed to view staff details");
            }

            if (currentLoginUserDTO.Position == StaffPosition.Admin.ToString())
            {
                return _mapper.Map<StaffDTO>(staff);
            }

            if (currentLoginUserDTO.Position == StaffPosition.Manager.ToString() && currentLoginUserDTO.Department != staff.Department.Name)
            {
                throw new APIException((int)HttpStatusCode.Forbidden, "Managers can only view staff in their own department");
            }

            if (currentLoginUserDTO.Position != StaffPosition.Manager.ToString())
            {
                throw new APIException((int)HttpStatusCode.Forbidden, "Only managers and admins can view staff details");
            }

            return _mapper.Map<StaffDTO>(staff);

        }

        public async Task<IEnumerable<StaffListingDTO>> GetAllStaffs()
        {
            var staffs = await _staffRepository.GetAll()
                                                .Include(department => department.Department)
                                                .ToListAsync();
            if (staffs == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Staff not found because it may have been deleted or does not exist.");
            }
            return _mapper.Map<IEnumerable<StaffListingDTO>>(staffs);
        }
    }
}
