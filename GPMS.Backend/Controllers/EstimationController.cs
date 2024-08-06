using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class EstimationController : ControllerBase
    {
        private readonly ILogger<EstimationController> _logger;
        private readonly IProductionEstimationService _productionEstimationService;

        public EstimationController(ILogger<EstimationController> logger, IProductionEstimationService productionEstimationService)
        {
            _logger = logger;
            _productionEstimationService = productionEstimationService;
        }

        [HttpPost]
        [Route(APIEndPoint.ESTIMATION_ID_OF_REQUIREMENT_ID_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all estimations by requirement ")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all estimation by requirement successfully", typeof(DefaultPageResponseListingDTO<ProductionEstimationListingDTO>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Production Estimation not found")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllEstimationByRequirements([FromRoute] Guid id, [FromBody] ProductionEstimationFilterModel productionEstimationFilterModel)
        {
            var response = await _productionEstimationService.GetAllEstimationOfRequirement(id, productionEstimationFilterModel);   
            return Ok(response);
        }
    }
}