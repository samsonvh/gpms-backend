using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Results;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Services;
using GPMS.Backend.Services.Services.Implementations;
using GPMS.Backend.Services.Utils;
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
        private readonly CurrentLoginUserDTO _currentLoginUser;

        public ProcessStepResultController(
            ILogger<ProcessStepResultController> logger, 
            IStepResultService stepResultService,
            CurrentLoginUserDTO currentLoginUser)
        {
            _logger = logger;
            _stepResultService = stepResultService;
            _currentLoginUser = currentLoginUser;
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
        [HttpPost]
        [Route(APIEndPoint.PRODUCTION_PROCESS_STEP_RESULT_OF_SERIES_ID_V1)]
        [SwaggerOperation(Summary = "Create Step Result with Input Output Result list")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Create Step Result with Input Output Result list successfully")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Create Step Result with Input Output Result list failed")]
        [Produces("application/json")]
        [Authorize("Staff")]
        public async Task<IActionResult> CreateStepResult([FromRoute] Guid id, [FromBody] StepResultInputDTO stepResultInputDTO)
        {
            _currentLoginUser.DecryptAccessToken(Request.Headers["Authorization"]);
            var response = await _stepResultService.Add(id, stepResultInputDTO); 
            return Ok(response);
        }
    }
}