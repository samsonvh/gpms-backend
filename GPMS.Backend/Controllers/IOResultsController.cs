using AutoMapper;
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
    [ApiController]
    public class IOResultsController : ControllerBase
    {
        private readonly IIOStepResultService _iOStepResultService;
        public IOResultsController(IIOStepResultService iOStepResultService)
        {
            _iOStepResultService = iOStepResultService;
        }

        [HttpPost]
        [Route(APIEndPoint.INPUT_OUTPUT_RESULT_OF_RESULT_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all input output result  by step result ")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all input output result by step result successfully", typeof(DefaultPageResponseListingDTO<IOResultListingDTO>))]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllIOResultsByStepResults([FromRoute] Guid id, [FromBody] IOResultFilterModel iOResultFilterModel)
        {
            var response = await _iOStepResultService.GetAllIOResultByStepResult(id, iOResultFilterModel);
            return Ok(response);
        }
    }
}
