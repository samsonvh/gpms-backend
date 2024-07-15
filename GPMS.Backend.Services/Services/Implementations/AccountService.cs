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
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<Account> _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IGenericRepository<Account> accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
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
    }
}
