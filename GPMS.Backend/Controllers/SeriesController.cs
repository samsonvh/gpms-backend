using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Services;
using GPMS.Backend.Services.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ILogger<SeriesController> _logger;
        private readonly IProductionSeriesService _productionSeriesService;

        public SeriesController(ILogger<SeriesController> logger, IProductionSeriesService productionSeriesService)
        {
            _logger = logger;
            _productionSeriesService = productionSeriesService;
        }

        [HttpPost]
        [Route(APIEndPoint.PRODUCTION_SERIES_OF_ESTIMATION_ID_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all series by estimation ")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all series by estimation successfully", typeof(DefaultPageResponseListingDTO<ProductionSeriesListingDTO>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Production Series not found")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllSeriesByEstimation([FromRoute] Guid id, [FromBody] ProductionSeriesFilterModel productionSeriesFilterModel)
        {
            var response = await _productionSeriesService.GetAllSeriesOfEstimation(id, productionSeriesFilterModel);
            return Ok(response);
        }

        [HttpPost]
        [Route(APIEndPoint.PRODUCTION_SERIES_OF_REQUIREMENT_ID_AND_DAY_NUMBER_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all series by requirementId and day number ")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all series by requirementId and day number successfully", typeof(DefaultPageResponseListingDTO<ProductionSeriesListingDTO>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Production Series not found")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllSeriesByRequirementIdAndDayNumber
            ([FromRoute] Guid id,[FromBody] ProductionSeriesFilterModel productionSeriesFilterModel)
        {
            var response = await _productionSeriesService.GetAllSeriesByRequirementIdAndDayNumber
                (id ,productionSeriesFilterModel);
            return Ok(response);
        }
    }
}