using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Services;
using GPMS.Backend.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using GPMS.Backend.Services.Services.Implementations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class ProductionPlanController : ControllerBase
    {
        private readonly IProductionPlanService _productionPlanService;

        private readonly ILogger<ProductionPlanController> _logger;

        public ProductionPlanController(IProductionPlanService productionPlanSerivce, ILogger<ProductionPlanController> logger)
        {
            _productionPlanService = productionPlanSerivce;
            _logger = logger;
        }

        [HttpPost]
        [Route(APIEndPoint.PRODUCTION_PLANS_V1)]
        [SwaggerOperation(Summary = "Add production plan using form")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Add Production Plan Successfully")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Add Production Plan Failed", typeof(List<FormError>))]
        [Produces("application/json")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddProductionPlans([FromBody] ProductionPlanInputDTO productionPlanInputDTO)
        {
            CurrentLoginUserDTO currentLoginUserDTO = JWTUtils.DecryptAccessToken(Request.Headers["Authorization"]);
            CreateUpdateResponseDTO<ProductionPlan> result = await _productionPlanService.Add(productionPlanInputDTO, currentLoginUserDTO);
            BaseReponse baseReponse = new BaseReponse
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = "Add Production Plan Successfully",
                Data = result
            };
            return Ok(baseReponse);
        }

        [HttpGet]
        [Route(APIEndPoint.PRODUCTION_PLANS_ID_V1)]
        [SwaggerOperation(Summary = "Get details of production plan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get details of production plan successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Production Plan not found")]
        [Produces("application/json")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            var productionPlan = await _productionPlanService.Details(id);
            return Ok(new BaseReponse { StatusCode = 200, Message = "Get details of production plan sucessfully", Data = productionPlan });
        }
    }
}