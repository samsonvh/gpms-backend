using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Requests;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GPMS.Backend.Services.Utils;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace GPMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionRequestsController : ControllerBase
    {
        private readonly IInspectionRequestService _inspectionRequestService;
        private readonly ILogger<InspectionRequest> _logger;
        private readonly CurrentLoginUserDTO _currentLoginUser;

        public InspectionRequestsController(IInspectionRequestService inspectionRequestService,
                                                ILogger<InspectionRequest> logger,
                                                CurrentLoginUserDTO currentLoginUser)
        {
            _inspectionRequestService = inspectionRequestService;
            _logger = logger;
            _currentLoginUser = currentLoginUser;
        }

        [HttpGet]
        [Route(APIEndPoint.INSPECTION_REQUEST_ID_V1)]
        [SwaggerOperation(Summary = "Get details of inspection request")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get details of inspection request successfully", typeof(InspectionRequestDTO))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Inspection Request not found")]
        [Produces("application/json")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            var result = await _inspectionRequestService.Details(id);
            return Ok(result);
        }

        [HttpPost]
        [Route(APIEndPoint.INSPECTION_REQUESTS_V1)]
        [SwaggerOperation(Summary = "Create inspection requests")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Create inspection request sucessfully", typeof(InspectionRequestDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid field inspection request")]
        [Produces("application/json")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([FromBody] InspectionRequestInputDTO inspectionRequestInputDTO)
        {
            _currentLoginUser.DecryptAccessToken(Request.Headers["Authorization"]);
            var result = await _inspectionRequestService.Add(inspectionRequestInputDTO);
            return Ok(result);
        }
    }
}
