using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Enums.Others;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs;
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
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<Account> _accountRepository;
        private readonly IGenericRepository<Staff> _staffRepository;
        private readonly IGenericRepository<Department> _departmentRepository;
        private readonly IValidator<AccountInputDTO> _accountInputDTOValidator;

        private readonly IMapper _mapper;

        public AccountService(IGenericRepository<Account> accountRepository,
                               IMapper mapper,
                               IValidator<AccountInputDTO> validator,
                               IGenericRepository<Staff> staffRepository,
                               IGenericRepository<Department> departmentRepository)
        {
            _accountRepository = accountRepository;
            _accountInputDTOValidator = validator;
            _staffRepository = staffRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<CreateUpdateResponseDTO<Account>> Add(AccountInputDTO inputDTO)
        {
            ValidationResult validateResult = await _accountInputDTOValidator.ValidateAsync(inputDTO);
            if (!validateResult.IsValid)
            {
                throw new ValidationException("Account Input Invalid", validateResult.Errors);
            }

            await CheckUniqueAccountCode(inputDTO.Code);
            await CheckUniqueAccountEmail(inputDTO.Email);
            /*await CheckValidDepartmentId(inputDTO.PersonalInfo.DepartmentId);*/

            if (inputDTO.PersonalInfo.Position != StaffPosition.FactoryDirector && inputDTO.PersonalInfo.Position != StaffPosition.Admin)
            {
                if (!inputDTO.PersonalInfo.DepartmentId.HasValue)
                {
                    throw new APIException(400, "Manager/Staff must be in one department");
                }
                await CheckValidDepartmentId(inputDTO.PersonalInfo.DepartmentId.Value);
            }
            else
            {
                inputDTO.PersonalInfo.DepartmentId = null;
            }

            //create account and staff
            var account = _mapper.Map<Account>(inputDTO);
            account.Status = AccountStatus.Active;
            account.Password = BCrypt.Net.BCrypt.HashPassword(inputDTO.Password);

            var staff = _mapper.Map<Staff>(inputDTO.PersonalInfo);
            staff.Status = StaffStatus.Active;
            staff.Code = account.Code;

            await CheckUniqueStaffCode(staff.Code);

            staff.Account = account;
            account.Staff = staff;

            _accountRepository.Add(account);
            _staffRepository.Add(staff);

            await _accountRepository.Save();
            await _staffRepository.Save();
            return _mapper.Map<CreateUpdateResponseDTO<Account>>(account);
        }

        private async Task CheckUniqueAccountCode(string code)
        {
            var existingAccountCode = await _accountRepository
                .Search(account => account.Code == code)
                .FirstOrDefaultAsync();

            if (existingAccountCode != null)
            {
                throw new APIException(400, "Account Code already exists");
            }
        }

        private async Task CheckUniqueAccountEmail(string email)
        {
            var existingAccountEmail = await _accountRepository
                .Search(account => account.Email == email)
                .FirstOrDefaultAsync();

            if (existingAccountEmail != null)
            {
                throw new APIException(400, "Account Email already exists");
            }
        }

        private async Task CheckUniqueStaffCode(string staffCode)
        {
            var existingStaff = await _staffRepository
                .Search(staff => staff.Code == staffCode)
                .FirstOrDefaultAsync();

            if (existingStaff != null)
            {
                throw new APIException(400, "Staff Code already exists.");
            }
        }

        private async Task CheckValidDepartmentId(Guid? departmentId)
        {
            if (departmentId.HasValue)
            {

                var existingDepartment = await _departmentRepository
                    .Search(department => department.Id == departmentId.Value)
                    .FirstOrDefaultAsync();

                if (existingDepartment == null)
                {
                    throw new APIException(400, "Invalid DepartmentId. Department not found.");
                }
            }
        }

        public async Task<IEnumerable<AccountListingDTO>> GetAllAccounts(AccountFilterModel accountFilterModel)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountDTO> Details(Guid id)
        {
            var account = await _accountRepository
                .Search(account => account.Id == id)
                .Include(account => account.Staff)
                .FirstOrDefaultAsync();
            if (account == null)
            {
                throw new APIException(404, "Account not found because it may have been deleted or does not exist.");
            }
            return _mapper.Map<AccountDTO>(account);
        }

        public async Task<ChangeStatusResponseDTO<Account, AccountStatus>> ChangeStatus(Guid id, string accountStatus)
        {
            var account = await _accountRepository
                .Search(account => account.Id == id)
                .Include(account => account.Staff)
                .FirstOrDefaultAsync();

            if (account == null)
            {
                throw new APIException(404, "Account not found because it may have been deleted or does not exist.");
            }

            if (account.Staff.Position == StaffPosition.Manager && account.Staff.Status == StaffStatus.In_production)
            {
                throw new APIException(400, "Cannot change status because the Production Manager is currently in production.");
            }

            if (!Enum.TryParse(accountStatus, true, out AccountStatus parsedStatus))
            {
                throw new APIException(400, "Invalid status value provided.");
            }

            account.Status = parsedStatus;

            //update staff status
            account.Staff.Status = ChangeStatusStaffAndAccount(parsedStatus);

            await _accountRepository.Save();
            await _staffRepository.Save();
            return _mapper.Map<ChangeStatusResponseDTO<Account, AccountStatus>>(account);
        }

        private StaffStatus ChangeStatusStaffAndAccount(AccountStatus accountStatus)
        {
            switch (accountStatus)
            {
                case AccountStatus.Active:
                    return StaffStatus.Active;
                case AccountStatus.Inactive:
                    return StaffStatus.Inactive;
                default:
                    throw new APIException(400, "Unsupported account status for changing staff status.");
            }
        }

        public Task AddList(List<AccountInputDTO> inputDTOs, Guid? parentEntityId = null)
        {
            throw new NotImplementedException();
        }

        public Task<CreateUpdateResponseDTO<Account>> Update(AccountInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<AccountInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponseListingDTO<AccountListingDTO>> GetAll(AccountFilterModel accountFilterModel)
        {
            var query = _accountRepository.GetAll();
            query = Filters(query, accountFilterModel);
            query = query.SortBy<Account>(accountFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery<Account>(accountFilterModel);
            var accounts = await query.Include(account => account.Staff).ProjectTo<AccountListingDTO>(_mapper.ConfigurationProvider)
                                        .ToListAsync();
            return new DefaultPageResponseListingDTO<AccountListingDTO>
            {
                Data = accounts,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = accountFilterModel.Pagination.PageIndex,
                    PageSize = accountFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }

        private IQueryable<Account> Filters(IQueryable<Account> query, AccountFilterModel accountFilterModel)
        {
            if (!accountFilterModel.Code.IsNullOrEmpty())
            {
                query = query.Where(account => account.Code.Contains(accountFilterModel.Code));
            }
            if (!accountFilterModel.Email.IsNullOrEmpty())
            {
                query = query.Where(account => account.Email.Contains(accountFilterModel.Email));
            }
            if (Enum.TryParse(accountFilterModel.AccountStatus, true, out AccountStatus accountStatus))
            {
                query = query.Where(account => account.Status.Equals(accountFilterModel.AccountStatus));
            }
            return query;
        }
    }
}
