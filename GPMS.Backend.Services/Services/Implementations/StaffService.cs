using AutoMapper;
using GPMS.Backend.Data.Configurations.EntityType;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<ChangeStatusResponseDTO<Staff, StaffStatus>> ChangeStatus(Guid id, StaffStatus newStatus)
        {
            var staff = await _staffRepository
               .Search(staff => staff.Id == id)
               .Include(staff => staff.Account)
               .FirstOrDefaultAsync();
            
            if (staff == null)
            {
                throw new APIException(404, "Staff not found because it may have been deleted or does not exist.");
            }

            if (staff.Status == StaffStatus.Inactive && newStatus == StaffStatus.In_production)
            {
                throw new APIException(400, "Staff has already been inactive so that cannot be changed to 'IN_PRODUCTION'.");
            }

            if (staff.Status == StaffStatus.In_production && newStatus == StaffStatus.Inactive)
            {
                throw new APIException(400, "Staff is currently in production so that cannot be inactive.");
            }

            staff.Status = newStatus;

            if (newStatus == StaffStatus.Inactive && staff.Account != null)
            {
                staff.Account.Status = AccountStatus.Inactive;
            }
            else if (newStatus == StaffStatus.Active)
            {
                staff.Account.Status = AccountStatus.Active;
            }

            await _staffRepository.Save();
            return _mapper.Map<ChangeStatusResponseDTO<Staff, StaffStatus>>(staff);
        }


        public async Task<IEnumerable<StaffListingDTO>> GetAllStaffs()
        {
            var staffs = await _staffRepository.GetAll().ToListAsync();
            if (staffs == null)
            {
                throw new APIException(404, "Staff not found because it may have been deleted or does not exist.");
            }
            return _mapper.Map<IEnumerable<StaffListingDTO>>(staffs);
        }
    }
}
