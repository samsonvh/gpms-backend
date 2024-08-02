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
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.DTOs.LisingDTOs;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        private readonly ILogger<StaffController> _logger;
        private readonly CurrentLoginUserDTO _currentLoginUser;

        public StaffController(IStaffService staffService,
        ILogger<StaffController> logger,
        CurrentLoginUserDTO currentLoginUser)
        {
            _staffService = staffService;
            _logger = logger;
            _currentLoginUser = currentLoginUser;
        }

        [HttpPost]
        [Route(APIEndPoint.STAFFS_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all staffs")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all staffs successfully", typeof(List<StaffListingDTO>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Staff not found")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllStaffs([FromBody] StaffFilterModel staffFilterModel)
        {
            var pageResponses = await _staffService.GetAll(staffFilterModel);
            return Ok(pageResponses);
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
            _currentLoginUser.DecryptAccessToken(Request.Headers["Authorization"]);
            var deparment = await _staffService.Details(id);
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