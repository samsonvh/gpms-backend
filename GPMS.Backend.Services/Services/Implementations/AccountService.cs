using AutoMapper;
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
using Microsoft.EntityFrameworkCore;
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
            await CheckUniqueStaffCode(inputDTO.StaffInputDTO.Code);
            await CheckValidDepartmentId(inputDTO.StaffInputDTO.DepartmentId);

            if (inputDTO.StaffInputDTO.Position != StaffPosition.FactoryDirector)
            {
                await CheckValidDepartmentId(inputDTO.StaffInputDTO.DepartmentId);
            }

            //create account and staff
            var account = _mapper.Map<Account>(inputDTO);
            account.Status = AccountStatus.Active;
            account.Password = BCrypt.Net.BCrypt.HashPassword(inputDTO.Password);

            var staff = _mapper.Map<Staff>(inputDTO.StaffInputDTO);
            staff.Status = StaffStatus.Active;

            if (inputDTO.StaffInputDTO.Position == StaffPosition.FactoryDirector && inputDTO.StaffInputDTO.DepartmentId != null)
            {
                throw new APIException(400, "Facotry Director must not in any deparment");
            }

            if (inputDTO.StaffInputDTO.Position != StaffPosition.FactoryDirector && inputDTO.StaffInputDTO.DepartmentId == null)
            {
                throw new APIException(400, "Manager/Staff must in 1 deparment");
            }

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

        public async Task<IEnumerable<AccountListingDTO>> GetAllAccounts()
        {
            var accounts = await _accountRepository.GetAll().ToListAsync();
            if (accounts == null)
            {
                throw new APIException(404, "Account not found because it may have been deleted or does not exist.");
            }
            return _mapper.Map<IEnumerable<AccountListingDTO>>(accounts);
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
    }
}
