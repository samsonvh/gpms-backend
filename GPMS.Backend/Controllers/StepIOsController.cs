using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Services;
using GPMS.Backend.Services.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace GPMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StepIOsController : ControllerBase
    {
        private readonly ILogger<StepIOsController> _logger;
        private readonly IStepIOService _stepIOService;

        public StepIOsController(ILogger<StepIOsController> logger, IStepIOService stepIOService)
        {
            _logger = logger;
            _stepIOService = stepIOService;
        }

        [HttpPost]
        [Route(APIEndPoint.STEP_INPUT_OUTPUT_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all production step input output ")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all step input output successfully", typeof(DefaultPageResponseListingDTO<StepIOListingDTO>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Step input output not found")]
        [Produces("application/json")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStepIOs([FromBody] StepIOFilterModel stepIOFilterModel)
        {
            var response = await _stepIOService.GetAll(stepIOFilterModel);
            return Ok(response);
        }

        [HttpPost]
        [Route(APIEndPoint.STEP_INPUT_OUTPUT_OF_STEP_ID_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all production step input output of step")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all step input output of step successfully", typeof(DefaultPageResponseListingDTO<StepIOListingDTO>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Step input output not found")]
        [Produces("application/json")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStepIOByStep([FromRoute] Guid id, [FromBody] StepIOFilterModel stepIOFilterModel)
        {
            var response = await _stepIOService.GetALlStepIOByStep(id, stepIOFilterModel);
            return Ok(response);
        }
        [HttpPost]
        [Route(APIEndPoint.STEP_INPUT_OUTPUT_OF_STEP_ID_V1 + "/step-results" + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all production step input output of step for step result")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all production step input output of step for step result successfully", typeof(PageResponseStepIOForStepResultListingDTO))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Step input output not found")]
        [Produces("application/json")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetALlStepIOByStepIdForStepResult([FromRoute] Guid id, [FromBody] StepIOFilterModel stepIOFilterModel)
        {
            var response = await _stepIOService.GetALlStepIOByStepIdForStepResult(id, stepIOFilterModel);
            return Ok(response);
        }
    }
}
