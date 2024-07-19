using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.Services;
using GPMS.Backend.Services.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using GPMS.Backend.Services.Utils;
using Microsoft.AspNetCore.Authorization;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly ILogger<StaffController> _logger;

        public StaffController(IStaffService staffService, ILogger<StaffController> logger)
        {
            _staffService = staffService;
            _logger = logger;
        }

        [HttpGet]
        [Route(APIEndPoint.STAFFS_V1)]
        [SwaggerOperation(Summary = "Get all staffs")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all staffs successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Department not found")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var staff = await _staffService.GetAllStaffs();
            return Ok(new BaseReponse { StatusCode = 200, Message = "Get all departments sucessfully", Data = staff });
        }

        [HttpGet]
        [Route(APIEndPoint.STAFFS_ID_V1)]
        [SwaggerOperation(Summary = "Get details of staff")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get staff details successfully")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Staff not found")]
        [SwaggerResponse((int)HttpStatusCode.Forbidden, "Access denied")]
        [Produces("application/json")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Details(Guid id)
        {
            CurrentLoginUserDTO currentLoginUserDTO = JWTUtils.DecryptAccessToken(Request.Headers["Authorization"]);
            var deparment = await _staffService.Details(id, currentLoginUserDTO);
            BaseReponse baseReponse = new BaseReponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Get details of staff successfully",
                Data = deparment
            };
            return Ok(baseReponse);
        }
    }
}