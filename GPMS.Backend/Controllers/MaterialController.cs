using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
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

        [HttpPost]
        [Route(APIEndPoint.MATERIAL_V1)]
        [SwaggerOperation(Summary = "Create Material")]
        [SwaggerResponse((int)HttpStatusCode.Created, "Create Material Successfully",typeof(MaterialDTO))]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateMaterial([FromBody] MaterialInputDTO materialInputDTO)
        {
            var response = await _materialService.Add(materialInputDTO);
            return CreatedAtAction(nameof(Details),new { id = response.Id }, response);
        }

        [HttpPut]
        [Route(APIEndPoint.MATERIAL_ID_V1)]
        [SwaggerOperation(Summary = "Update Material By Id")]
        [SwaggerResponse((int)HttpStatusCode.Created, "Update Material Successfully",typeof(MaterialDTO))]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateMaterial([FromRoute] Guid id,[FromBody] MaterialInputDTO materialInputDTO)
        {
            var response = await _materialService.Update(id,materialInputDTO);
            return Ok(response);
        }

        [HttpPost]
        [Route(APIEndPoint.MATERIAL_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get All Material")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Material List", typeof(DefaultPageResponseListingDTO<MaterialListingDTO>))]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllMaterial([FromBody] MaterialFilterModel materialFilterModel)
        {
            var response = await _materialService.GetAll(materialFilterModel);
            return Ok(response);
        }

        [HttpGet]
        [Route(APIEndPoint.MATERIAL_ID_V1)]
        [SwaggerOperation(Summary = "Get details of material")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get details of material successfully", typeof(MaterialDTO))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Material not found")]
        [Produces("application/json")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            var material = await _materialService.Details(id);
            return Ok(material);
        }
    }
}