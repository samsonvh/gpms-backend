using AutoMapper;
using AutoMapper.QueryableExtensions;
using GPMS.Backend.Data.Enums.Others;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Utils;
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

        public async Task<DepartmentDTO> Details(Guid id)
        {
            var department = await _departmentRepository
                .Search(department => department.Id == id)
                .Include(department => department.Staffs)
                .FirstOrDefaultAsync();

            if (department == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Department not found");
            }

            /*var currentStaff = await _staffRepository
                .Search(staff => staff.Id == currentLoginUserDTO.Id)
                .FirstOrDefaultAsync();*/

            /*if (currentStaff == null)
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
            }*/

            var departmentDTO = _mapper.Map<DepartmentDTO>(department);
            return departmentDTO;
        }


        public async Task<DefaultPageResponseListingDTO<DepartmentListingDTO>> GetAllDepartments(DepartmentFilterModel departmentFilterModel)
        {
            var query = _departmentRepository.GetAll();
            query = query.SortBy<Department>(departmentFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery<Department>(departmentFilterModel);
            var departments = await query.ProjectTo<DepartmentListingDTO>(_mapper.ConfigurationProvider)
                                        .ToListAsync();
            return new DefaultPageResponseListingDTO<DepartmentListingDTO>
            {
                Data = departments,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = departmentFilterModel.Pagination.PageIndex,
                    PageSize = departmentFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }


    }
}
