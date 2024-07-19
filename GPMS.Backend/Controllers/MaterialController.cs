using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Services;
using GPMS.Backend.Services.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly ILogger<MaterialController> _logger;
        private readonly IMaterialService _materialService;

        public MaterialController(ILogger<MaterialController> logger, IMaterialService materialService)
        {
            _logger = logger;
            _materialService = materialService;
        }

        [HttpGet]
        [Route(APIEndPoint.MATERIAL_V1)]
        [SwaggerOperation(Summary = "Get All Material")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Material List")]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllMaterial ()
        {
            var data = await _materialService.GetAll();
            BaseReponse response = new BaseReponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Material List",
                Data = data
            };
            return Ok(response);
        }

        [HttpGet]
        [Route(APIEndPoint.MATERIAL_ID_V1)]
        [SwaggerOperation(Summary = "Get details of material")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get details of material successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Material not found")]
        [Produces("application/json")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            var material = await _materialService.Details(id);
            return Ok(new BaseReponse { StatusCode = 200, Message = "Get details of material sucessfully", Data = material });
        }
    }
}