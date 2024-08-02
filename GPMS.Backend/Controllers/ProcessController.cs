using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.PageRequests;
using GPMS.Backend.Services.Services;
using GPMS.Backend.Services.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private readonly ILogger<ProcessController> _logger;
        private readonly IProcessService _processService;

        public ProcessController(ILogger<ProcessController> logger, IProcessService processService)
        {
            _logger = logger;
            _processService = processService;
        }

        [HttpPost]
        [Route(APIEndPoint.PROCESS_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all processes")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all processes successfully", typeof(DefaultPageResponseListingDTO<ProcessListingDTO>))]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllProcesses([FromBody] ProcessFilterModel processFilterModel)
        {
            DefaultPageResponseListingDTO<ProcessListingDTO> pageResponse = await _processService.GetAll(processFilterModel);
            return Ok(pageResponse);
        }

        [HttpPost]
        [Route(APIEndPoint.PROCESSES_OF_PRODUCT_ID_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all processes of a specific product")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all processes of a product successfully", typeof(DefaultPageResponseListingDTO<ProcessListingDTO>))]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllProcessesOfProduct([FromRoute] Guid id, [FromBody] ProcessFilterModel processFilterModel)
        {
            DefaultPageResponseListingDTO<ProcessListingDTO> pageResponse = await _processService.GetAllProcessOfProduct(id, processFilterModel);
            return Ok(pageResponse);
        }
    }
}