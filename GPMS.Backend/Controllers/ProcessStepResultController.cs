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
using GPMS.Backend.Services.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class ProcessStepResultController : ControllerBase
    {
        private readonly ILogger<ProcessStepResultController> _logger;
        private readonly IStepResultService _stepResultService;

        public ProcessStepResultController(ILogger<ProcessStepResultController> logger, IStepResultService stepResultService)
        {
            _logger = logger;
            _stepResultService = stepResultService;
        }

        [HttpPost]
        [Route(APIEndPoint.PRODUCTION_PROCESS_STEP_RESULT_OF_SERIES_ID_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all production process step sucesssfully")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all production process step successfully", typeof(DefaultPageResponseListingDTO<StepResultListingDTO>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Step Result not found")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllStepResultBySeries([FromRoute] Guid id, [FromBody] StepResultFilterModel stepResultFilterModel)
        {
            var response = await _stepResultService.GetALlStepResultBySeries(id, stepResultFilterModel);
            return Ok(response);
        }
    }
}