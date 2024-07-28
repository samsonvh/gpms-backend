using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Data.Enums.Statuses.Requests;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Requests;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
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
    public class WarehouseRequestController : ControllerBase
    {
        private readonly IWarehouseRequestService _warehouseRequestService;
        private readonly ILogger<WarehouseRequestController> _logger;

        public WarehouseRequestController(IWarehouseRequestService warehouseRequestService, ILogger<WarehouseRequestController> logger)
        {
            _warehouseRequestService = warehouseRequestService;
            _logger = logger;
        }

        [HttpPost]
        [Route(APIEndPoint.WAREHOUSE_REQUESTS_OF_REQUIREMENT_ID_V1)]
        [SwaggerOperation(Summary = "Create warehouse request")]
        [SwaggerResponse((int)HttpStatusCode.Created, "Create warehouse request successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid Data")]
        [Produces("application/json")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(WarehouseRequestInputDTO warehouseRequestInputDTO)
        {
            CurrentLoginUserDTO currentLoginUserDTO = JWTUtils.DecryptAccessToken(Request.Headers["Authorization"]);
            var createdWarehouseRequest = await _warehouseRequestService.Add(warehouseRequestInputDTO,currentLoginUserDTO);

            var responseData = new CreateUpdateResponseDTO<WarehouseRequest>
            {
                Id = createdWarehouseRequest.Id,
                Code = createdWarehouseRequest.Code
            };

            BaseReponse baseReponse = new BaseReponse
            {
                StatusCode = 201,
                Message = "Create warehouse request sucessfully",
                Data = responseData
            };
            return CreatedAtAction(nameof(Create), baseReponse);
        }

        [HttpPatch]
        [Route(APIEndPoint.WAREHOUSE_REQUEST_ID_OF_REQUIREMENT_ID_V1)]
        [SwaggerOperation(Summary = "Approve/Decline warehouse request")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Change stauts of warehouse request successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid status")]
        [Produces("application/json")]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] ChangeStatusInputDTO status)
        {
            CurrentLoginUserDTO currentLoginUserDTO = JWTUtils.DecryptAccessToken(Request.Headers["Authorization"]);
            var warehouseRequest = await _warehouseRequestService.ChangeStatus(id, status, currentLoginUserDTO);
            var responseData = new ChangeStatusResponseDTO<WarehouseRequest, WarehouseRequestStatus>
            {
                Id = warehouseRequest.Id,
                Status = warehouseRequest.Status,
            };

            return Ok(new BaseReponse { StatusCode = 200, Message = "Change status of warehouse request sucessfully", Data = responseData });
        }

        [HttpGet]
        [Route(APIEndPoint.WAREHOUSE_REQUEST_ID_OF_REQUIREMENT_ID_V1)]
        [SwaggerOperation(Summary = "Get details of warehouse request")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get details of warehouse request successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Warehouse request not found")]
        [Produces("application/json")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            CurrentLoginUserDTO currentLoginUserDTO = JWTUtils.DecryptAccessToken(Request.Headers["Authorization"]);
            var warehouseRequest = await _warehouseRequestService.Details(id, currentLoginUserDTO);
            return Ok(new BaseReponse { StatusCode = 200, Message = "Get details of warehouse request sucessfully", Data = warehouseRequest });
        }
    }
}