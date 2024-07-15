using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all accounts successfully", typeof(AccountDTO))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Account not found")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var account = await _accountService.GetAllAccounts();
            return Ok(new BaseReponse { StatusCode = 200, Message = "Get all accounts sucessfully", Data = account });
        }
    }
}