using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class RequirementController : ControllerBase
    {
        private readonly ILogger<RequirementController> _logger;
        private readonly IProductionRequirementService _productionRequirementService;

        public RequirementController(ILogger<RequirementController> logger, IProductionRequirementService productionRequirementService)
        {
            _logger = logger;
            _productionRequirementService = productionRequirementService;
        }
        [HttpPost]
        [Route(APIEndPoint.REQUIREMENTS_OF_PRODUCTION_PLAN_ID_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all production requirement by production plan Id", Description = "Factory director, Production manager can get all production requirement by production plan id")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all production requirement by production plan Id successfully")]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllProductionRequirementByProductionPlanId([FromBody] RequirementFilterModel requirementFilterModel, [FromRoute] Guid id)
        {
            return Ok(await _productionRequirementService.GetAllByProductionPlanId(id,requirementFilterModel));
        }
        [HttpPost]
        [Route(APIEndPoint.REQUIREMENTS_OF_PRODUCTION_PLAN_ID_V1 + APIEndPoint.STEP_INPUT_OUTPUT + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all production requirement by production plan Id for view step input output info of in progress production plan", Description = "Factory director, Production manager can Get all production requirement by production plan Id for view step input output info of in progress production plan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all production requirement by production plan Id successfully")]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllRequirementHaveAvailableSeriesAtCurrentDayByProductionPlanId([FromBody] RequirementFilterModel requirementFilterModel, [FromRoute] Guid id)
        {
            return Ok(await _productionRequirementService.GetAllRequirementHaveAvailableSeriesAtCurrentDayByProductionPlanId(id,requirementFilterModel));
        }
    }
}