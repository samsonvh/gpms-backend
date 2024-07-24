using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Requests;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Services;
using GPMS.Backend.Services.Services.Implementations;
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
        public async Task<IActionResult> Create(WarehouseRequestInputDTO warehouseRequestInputDTO)
        {
            var createdWarehouseRequest = await _warehouseRequestService.Add(warehouseRequestInputDTO);

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
    }
}