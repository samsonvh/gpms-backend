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
using Microsoft.AspNetCore.Authorization;
using GPMS.Backend.Services.Utils;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;

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

        [HttpGet]
        [Route(APIEndPoint.STAFFS_OF_DEPARTMENT_ID_V1)]
        [SwaggerOperation(Summary = "Get details of deparment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get department details successfully")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Department not found")]
        [SwaggerResponse((int)HttpStatusCode.Forbidden, "Access denied")]
        [Produces("application/json")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Details(Guid id)
        {
            CurrentLoginUserDTO currentLoginUserDTO = JWTUtils.DecryptAccessToken(Request.Headers["Authorization"]);
            var deparment = await _departmentService.Details(id, currentLoginUserDTO);
            BaseReponse baseReponse = new BaseReponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Get details of deparment successfully",
                Data = deparment
            };
            return Ok(baseReponse);
        }
    }
}