using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Services;
using GPMS.Backend.Services.Utils;
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
        private readonly CurrentLoginUserDTO _currentLoginUserDTO;
        public AuthenticationController(ILogger<AuthenticationController> logger, 
        IAuthenticationService authenticationService, CurrentLoginUserDTO currentLoginUserDTO)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _currentLoginUserDTO = currentLoginUserDTO;
        }
        [HttpPost]
        [Route(APIEndPoint.AUTH_SIGN_IN_V1)]
        [SwaggerOperation(Summary = "Login to system using email and password")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Login Successfully", typeof(LoginResponseDTO))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized")]
        [SwaggerResponse((int)HttpStatusCode.Forbidden, "Forbidden")]
        [Produces("application/json")]
        public async Task<IActionResult> LoginWithCredential([FromBody] LoginInputDTO loginInputDTO)
        {
            LoginResponseDTO baseReponse = await _authenticationService.SignInWIthEmailPassword(loginInputDTO);
            return Ok(baseReponse);
        }
        [HttpGet]
        [Route("api/v1/getpasswordhashed")]
        public async Task<IActionResult> GetPasswordHashed([FromQuery] string password)
        {
            return Ok(BCrypt.Net.BCrypt.HashPassword(password));
        }
        [HttpPost]
        [Route(APIEndPoint.AUTH_SIGN_IN_WITH_TOKEN_V1)]
        public async Task<IActionResult> SignInWithToken()
        {
            _currentLoginUserDTO.DecryptAccessToken(Request.Headers["Authorization"]);  
            return Ok(_currentLoginUserDTO);
        }
    }
}