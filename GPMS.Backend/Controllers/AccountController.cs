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
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Services;
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

        [HttpGet]
        [Route(APIEndPoint.ACCOUNTS_V1)]
        [SwaggerOperation(Summary = "Get all accounts")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all accounts successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Account not found")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var account = await _accountService.GetAllAccounts();
            return Ok(new BaseReponse { StatusCode = 200, Message = "Get all accounts sucessfully", Data = account });
        }

        [HttpGet]
        [Route(APIEndPoint.ACCOUNTS_ID_V1)]
        [SwaggerOperation(Summary = "Get details of account")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get details of account successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Account not found")]
        [Produces("application/json")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            var account = await _accountService.Details(id);
            return Ok(new BaseReponse { StatusCode = 200, Message = "Get details of account sucessfully", Data = account });
        }

        [HttpPost]
        [Route(APIEndPoint.ACCOUNTS_V1)]
        [SwaggerOperation(Summary = "Provide account")]
        [SwaggerResponse((int)HttpStatusCode.Created, "Provide account successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid Data")]
        [Produces("application/json")]
        public async Task<IActionResult> Create(AccountInputDTO accountInputDTO)
        {
            var createdAccount = await _accountService.Add(accountInputDTO);
                
            var responseData = new CreateUpdateResponseDTO<Account>
            {
                Id = createdAccount.Id,
                Code = createdAccount.Code
            };

            BaseReponse baseReponse = new BaseReponse
            {
                StatusCode = 201,
                Message = "Provide account sucessfully",
                Data = responseData
            };
            return CreatedAtAction(nameof(Create), baseReponse);
        }

        [HttpPatch]
        [Route(APIEndPoint.ACCOUNTS_ID_V1)]
        [SwaggerOperation(Summary = "Change status of account")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Change stauts of account successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Account not found")]
        [Produces("application/json")]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            var account = await _accountService.ChangeStatus(id, status);
            var responseData = new ChangeStatusResponseDTO<Account, AccountStatus>
            {
                Id = account.Id,
                Status = account.Status
            };

            return Ok(new BaseReponse { StatusCode = 200, Message = "Change status of account sucessfully", Data = responseData });
        }
    }
}