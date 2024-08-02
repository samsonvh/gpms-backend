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
    public class QualityStandardsController : ControllerBase
    {
        private readonly IQualityStandardService _qualityStandardService;
        private readonly ILogger<QualityStandardsController> _logger;

        public QualityStandardsController(ILogger<QualityStandardsController> logger, IQualityStandardService qualityStandardService)
        {
            _logger = logger;
            _qualityStandardService = qualityStandardService;
        }

        [HttpPost]
        [Route(APIEndPoint.QUALITY_STANDARD_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all quality standard", Description = "Factory director, Production manager can get all quality standard")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all specification successfully", typeof(DefaultPageResponseListingDTO<QualityStandardListingDTO>))]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllQualityStandards([FromBody] QualityStandardFilterModel qualityStandardFilterModel)
        {
            var pageResponses = await _qualityStandardService.GetAll(qualityStandardFilterModel);

            return Ok(pageResponses);
        }
    }
}
