using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azure;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost]
        [Route(APIEndPoint.ACCOUNTS_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all accounts")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all accounts successfully", typeof(DefaultPageResponseListingDTO<AccountListingDTO>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Account not found")]
        [Produces("application/json")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccounts([FromBody] AccountFilterModel accountFilterModel)
        {
            var response = await _accountService.GetAll(accountFilterModel);
            return Ok(response);
        }

        [HttpGet]
        [Route(APIEndPoint.ACCOUNTS_ID_V1)]
        [SwaggerOperation(Summary = "Get details of account")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get details of account successfully", typeof(AccountDTO))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Account not found")]
        [Produces("application/json")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            var account = await _accountService.Details(id);
            return Ok(account);
        }

        [HttpPost]
        [Route(APIEndPoint.ACCOUNTS_V1)]
        [SwaggerOperation(Summary = "Provide account")]
        [SwaggerResponse((int)HttpStatusCode.Created, "Provide account successfully", typeof(AccountDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid Data")]
        [Produces("application/json")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(AccountInputDTO accountInputDTO)
        {
            var createdAccount = await _accountService.Add(accountInputDTO);
            
            return CreatedAtAction(
                nameof(Create),
                new { id = createdAccount.Id }, 
                createdAccount);
        }

        [HttpPatch]
        [Route(APIEndPoint.ACCOUNTS_ID_V1)]
        [SwaggerOperation(Summary = "Change status of account")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Change stauts of account successfully", typeof(AccountDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid status")]
        [Produces("application/json")]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] AccountInputStatus status)
        {
            var account = await _accountService.ChangeStatus(id, status);
            return Ok(account);
        }
    }
}