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
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Staffs;

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

        [HttpPatch]
        [Route(APIEndPoint.STAFFS_ID_V1)]
        [SwaggerOperation(Summary = "Change stauts staff")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Change status staff successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid status")]
        [Produces("application/json")]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] StaffStatus status)
        {
            var staff = await _staffService.ChangeStatus(id, status);
            var responseData = new ChangeStatusResponseDTO<Staff, StaffStatus>
            {
                Id = staff.Id,
                Status = staff.Status,
            };

            return Ok(new BaseReponse { StatusCode = 200, Message = "Change status of account sucessfully", Data = responseData });
        }
    }
}