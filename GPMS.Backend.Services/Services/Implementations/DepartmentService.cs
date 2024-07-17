using AutoMapper;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IGenericRepository<Department> _departmentRepository;

        private readonly IMapper _mapper;
        public DepartmentService(IMapper mapper, IGenericRepository<Department> departmentRepository)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
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
