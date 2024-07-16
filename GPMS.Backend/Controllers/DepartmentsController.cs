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

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly IDepartmentService _departmentService;

        public DepartmentController(ILogger<DepartmentController> logger, IDepartmentService departmentService)
        {
            _logger = logger;
            _departmentService = departmentService;
        }

        [HttpGet]
        [Route(APIEndPoint.DEPARTMENTS_V1)]
        [SwaggerOperation(Summary = "Get all departments")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all departments successfully", typeof(AccountDTO))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Department not found")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var department = await _departmentService.GetAllDepartments();
            return Ok(new BaseReponse { StatusCode = 200, Message = "Get all departments sucessfully", Data = department });
        }
    }
}