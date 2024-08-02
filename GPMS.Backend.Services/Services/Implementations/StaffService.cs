using AutoMapper;
using AutoMapper.QueryableExtensions;
using GPMS.Backend.Data.Enums.Others;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        private readonly CurrentLoginUserDTO _currentLoginUser;

        public StaffService(IGenericRepository<Staff> staffRepository,
        IMapper mapper,
        CurrentLoginUserDTO currentLoginUser)
        {
            _staffRepository = staffRepository;
            _mapper = mapper;
            _currentLoginUser = currentLoginUser;
        }

        public async Task<StaffDTO> Details(Guid id)
        {
            var staff = await _staffRepository
                .Search(staff => staff.Id == id)
                .Include(staff => staff.Department)
                .FirstOrDefaultAsync();

            if (staff == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Staff not found");
            }

            if (_currentLoginUser.Position == StaffPosition.FactoryDirector.ToString())
            {
                throw new APIException((int)HttpStatusCode.Forbidden, "Factory directors are not allowed to view staff details");
            }

            if (_currentLoginUser.Position == StaffPosition.Admin.ToString())
            {
                return _mapper.Map<StaffDTO>(staff);
            }

            if (_currentLoginUser.Position == StaffPosition.Manager.ToString() && _currentLoginUser.Department != staff.Department.Name)
            {
                throw new APIException((int)HttpStatusCode.Forbidden, "Managers can only view staff in their own department");
            }

            if (_currentLoginUser.Position != StaffPosition.Manager.ToString())
            {
                throw new APIException((int)HttpStatusCode.Forbidden, "Only managers and admins can view staff details");
            }

            return _mapper.Map<StaffDTO>(staff);

        }

        public async Task<DefaultPageResponseListingDTO<StaffListingDTO>> GetAll(StaffFilterModel staffFilterModel)
        {
            var query = _staffRepository.GetAll();
            query = Filters(query, staffFilterModel);
            query = query.SortBy<Staff>(staffFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery<Staff>(staffFilterModel);
            var staffs = await query.ProjectTo<StaffListingDTO>(_mapper.ConfigurationProvider)
                                        .ToListAsync();
            return new DefaultPageResponseListingDTO<StaffListingDTO>
            {
                Data = staffs,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = staffFilterModel.Pagination.PageIndex,
                    PageSize = staffFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }

        private IQueryable<Staff> Filters(IQueryable<Staff> query, StaffFilterModel staffFilterModel)
        {
            if (!staffFilterModel.Code.IsNullOrEmpty())
            {
                query = query.Where(process => process.Code.Contains(staffFilterModel.Code));
            }

            if (Enum.TryParse(staffFilterModel.Position, true, out StaffPosition staffPosition))
            {
                query = query.Where(staff => staff.Position.Equals(staffFilterModel.Position));
            }

            if (Enum.TryParse(staffFilterModel.Status, true, out StaffStatus staffStatus))
            {
                query = query.Where(staff => staff.Status.Equals(staffFilterModel.Status));
            }
            return query;
        }

    }
}
