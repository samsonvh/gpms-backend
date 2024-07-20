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

namespace GPMS.Backend.Services.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IGenericRepository<Staff> _staffRepository;

        private readonly IMapper _mapper;
        public DepartmentService(IMapper mapper, IGenericRepository<Department> departmentRepository, IGenericRepository<Staff> staffRepository)
        {
            _departmentRepository = departmentRepository;
            _staffRepository = staffRepository;
            _mapper = mapper;
        }

        public async Task<DepartmentDTO> Details(Guid id, CurrentLoginUserDTO currentLoginUserDTO)
        {
            var department = await _departmentRepository
                .Search(department => department.Id == id)
                .Include(department => department.Staffs)
                .FirstOrDefaultAsync();

            if (department == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Department not found");
            }

            var currentStaff = await _staffRepository
                .Search(staff => staff.Id == currentLoginUserDTO.StaffId)
                .FirstOrDefaultAsync();

            if (currentStaff == null)
            {
                throw new APIException((int)HttpStatusCode.Forbidden, "Current staff not found");
            }

            if (currentStaff.Position == StaffPosition.FactoryDirector)
            {
                throw new APIException((int)HttpStatusCode.Forbidden, "Factory directors are not allowed to view department details");
            }

            if (currentStaff.Position == StaffPosition.Admin)
            {
                return _mapper.Map<DepartmentDTO>(department);
            }

            if (currentStaff.Position == StaffPosition.Manager && currentStaff.DepartmentId != id)
            {
                throw new APIException((int)HttpStatusCode.Forbidden, "Managers can only view their own department");
            }

            if (currentStaff.Position != StaffPosition.Manager)
            {
                throw new APIException((int)HttpStatusCode.Forbidden, "Only managers and admins can view department details");
            }

            var departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return departmentDTO;
        }


        public async Task<IEnumerable<DepartmentListingDTO>> GetAllDepartments()
        {
            var deparments = await _departmentRepository.GetAll().ToListAsync();
            if (deparments == null)
            {
                throw new APIException(404, "Deparment not found because it may have been deleted or does not exist.");
            }
            return _mapper.Map<IEnumerable<DepartmentListingDTO>>(deparments);
        }
    }
}
