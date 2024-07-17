using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }
        [HttpPost]
        [Route(APIEndPoint.AUTHENTICATION_CREDENTIALS_V1)]
        [SwaggerOperation(Summary = "Login to system using email and password")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Login Successfully", typeof(LoginResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized")]
        [SwaggerResponse((int)HttpStatusCode.Forbidden, "Forbidden")]
        [Produces("application/json")]
        public async Task<IActionResult> LoginWithCredential([FromBody] LoginInputDTO loginInputDTO)
        {
            LoginResponseDTO loginResponseDTO = await _authenticationService.LoginWithCredential(loginInputDTO);
            BaseReponse baseReponse = new BaseReponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Login Successfully",
                Data = loginResponseDTO
            };
            return Ok(baseReponse);
        }
        [HttpGet]
        [Route("api/v1/getpasswordhashed")]
        public async Task<IActionResult> GetPasswordHashed([FromQuery] string password)
        {
            return Ok(BCrypt.Net.BCrypt.HashPassword(password));
        }
        [HttpGet]
        [Route("api/v1/authentication/testauthorize")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> TestAuthorize()
        {
            
            return Ok("Test Authorize Successfully");
        }
    }
}