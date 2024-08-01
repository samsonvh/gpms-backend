﻿using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services
{
    public interface IAccountService : IBaseService<AccountInputDTO,CreateUpdateResponseDTO<Account>,AccountListingDTO,AccountDTO,AccountFilterModel>
    {
        Task<IEnumerable<AccountListingDTO>> GetAllAccounts(AccountFilterModel accountFilterModel);
        Task<CreateUpdateResponseDTO<Account>> Add(AccountInputDTO inputDTO);
        Task<AccountDTO> Details(Guid id);
        Task<ChangeStatusResponseDTO<Account, AccountStatus>> ChangeStatus(Guid id, string accountStatus);
    }
}
