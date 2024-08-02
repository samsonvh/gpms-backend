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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class StepController : ControllerBase
    {
        private readonly ILogger<StepController> _logger;
        private readonly IStepService _stepService;

        public StepController(ILogger<StepController> logger, IStepService stepService)
        {
            _logger = logger;
            _stepService = stepService;
        }

        [HttpPost]
        [Route(APIEndPoint.STEPS_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all steps")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all steps successfully", typeof(DefaultPageResponseListingDTO<StepListingDTO>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Step not found")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllSteps([FromBody] StepFilterModel stepFilterModel)
        {
            var response = await _stepService.GetAll(stepFilterModel);
            return Ok(response);
        }

    }
}