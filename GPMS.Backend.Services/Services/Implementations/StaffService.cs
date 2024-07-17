using AutoMapper;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
