using System.Net;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Enums.Others;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IGenericRepository<Account> _accountRepository;
        private readonly IValidator<LoginInputDTO> _loginValidator;
        public AuthenticationService(IGenericRepository<Account> accountRepository, IValidator<LoginInputDTO> loginValidator)
        {
            _accountRepository = accountRepository;
            _loginValidator = loginValidator;
        }
        public async Task<LoginResponseDTO> LoginWithCredential(LoginInputDTO loginInputDTO)
        {
            ValidationResult validateResult = await _loginValidator.ValidateAsync(loginInputDTO);
            if (!validateResult.IsValid)
            {
                throw new ValidationException("Login Input Invalid", validateResult.Errors);
            }
            IQueryable<Account> query = _accountRepository.Search(account => account.Email.Equals(loginInputDTO.Email))
                                                            .Include(account => account.Staff)
                                                            .Include(account => account.Staff.Department);
            Account existedAccount = await query.FirstOrDefaultAsync();
            if (existedAccount == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Account Not Found");
            }
            if (existedAccount.Staff == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Staff In Account Null");
            }
            if(!existedAccount.Staff.Position.Equals(StaffPosition.Admin) && !existedAccount.Staff.Position.Equals(StaffPosition.FactoryDirector) && existedAccount.Staff.Department == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Account is not admin must have deparment");
            }
            if (existedAccount.Status == AccountStatus.Inactive || existedAccount.Staff.Status == StaffStatus.Inactive)
            {
                throw new APIException((int)HttpStatusCode.Unauthorized, "Account Is Inactive, Can Not Login");
            }
            if (!BCrypt.Net.BCrypt.Verify(loginInputDTO.Password,existedAccount.Password))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Wrong Email Or Password");
            }
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO
            {
                Code = existedAccount.Code,
                FullName = existedAccount.Staff.FullName,
                Position = existedAccount.Staff.Position.ToString(),
                Department = (existedAccount.Staff.Position.Equals(StaffPosition.Admin) || existedAccount.Staff.Position.Equals(StaffPosition.FactoryDirector)) ? null : existedAccount.Staff.Department.Name,
                Token = JWTUtils.GenerateJWTToken(existedAccount)
            }; 
            return loginResponseDTO;
        }
    }
}