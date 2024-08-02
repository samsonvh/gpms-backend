using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.PageRequests;
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
    public class MeasurementsController : ControllerBase
    {
        private readonly IMeasurementService _measurementService;
        private readonly ILogger<MeasurementsController> _logger;

        public MeasurementsController(IMeasurementService measurementService,ILogger<MeasurementsController> logger)
        {
            _measurementService = measurementService;
            _logger = logger;
        }

        [HttpPost]
        [Route(APIEndPoint.MEASUREMENT_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all measurement")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all measurement successfully", typeof(List<MeasurementListingDTO>))]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllMeasurements([FromBody] MeasurementFilterModel measurementFilterModel)
        {
            var pageResponses = await _measurementService.GetAll(measurementFilterModel);
            return Ok(pageResponses);
        }
    }
}
