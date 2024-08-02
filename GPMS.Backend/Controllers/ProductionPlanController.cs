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
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.PageRequests;
using GPMS.Backend.Data.Enums.Types;
using FluentValidation;
using System.Reflection.Metadata.Ecma335;
using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
using GPMS.Backend.Data.Enums.Others;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class ProductionPlanController : ControllerBase
    {
        private readonly IProductionPlanService _productionPlanService;

        private readonly ILogger<ProductionPlanController> _logger;
        private readonly CurrentLoginUserDTO _currentLoginUserDTO;

        public ProductionPlanController(
            IProductionPlanService productionPlanSerivce,
            ILogger<ProductionPlanController> logger,
            CurrentLoginUserDTO currentLoginUserDTO)
        {
            _productionPlanService = productionPlanSerivce;
            _logger = logger;
            _currentLoginUserDTO = currentLoginUserDTO;
        }

        [HttpPost]
        [Route(APIEndPoint.PRODUCTION_PLANS_V1)]
        [SwaggerOperation(Summary = "Add production plan using form")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Create Production Plan Successfully")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Create Production Plan List Failed", typeof(List<FormError>))]
        [Produces("application/json")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddProductionPlans([FromBody] List<ProductionPlanInputDTO> productionPlanInputDTOs)
        {
            _currentLoginUserDTO.DecryptAccessToken(Request.Headers["Authorization"]);
            List<CreateUpdateResponseDTO<ProductionPlan>> result = await ClassifyAndAddProductionPlan(productionPlanInputDTOs);
            BaseReponse baseReponse = new BaseReponse
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = "Create Production Plan Successfully",
                Data = result
            };
            return Ok(baseReponse);
        }

        private async Task<List<CreateUpdateResponseDTO<ProductionPlan>>> ClassifyAndAddProductionPlan
            (List<ProductionPlanInputDTO> inputDTOs)
        {
            List<CreateUpdateResponseDTO<ProductionPlan>> result = new List<CreateUpdateResponseDTO<ProductionPlan>>();
            List<ProductionPlanInputDTO> yearProductionPlanList = new List<ProductionPlanInputDTO>();
            List<ProductionPlanInputDTO> childProductionPlanList = new List<ProductionPlanInputDTO>();
            foreach (ProductionPlanInputDTO inputDTO in inputDTOs)
            {
                if (inputDTO.Type.ToLower().Equals(ProductionPlanType.Year.ToString().ToLower()))
                {
                    yearProductionPlanList.Add(inputDTO);
                }
                else
                {
                    childProductionPlanList.Add(inputDTO);
                }
            }
            if (yearProductionPlanList.Count > 0)
            {
                result = await _productionPlanService.AddAnnualProductionPlanList(yearProductionPlanList);
            }
            else if (childProductionPlanList.Count > 0)
            {
                result = await _productionPlanService.AddChildProductionPlanList(childProductionPlanList);
            }
            else
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Production Plan List Is Required");
            }
            return result;
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

        [HttpPost]
        [Route(APIEndPoint.PRODUCTION_PLANS_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all production plan", Description = "Factory director, Production manager can get all production plan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all production plan successfully", typeof(List<ProductionPlanListingDTO>))]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllProductionPlans([FromBody] ProductionPlanFilterModel productionPlanFilterModel)
        {
            DefaultPageResponseListingDTO<ProductionPlanListingDTO> pageResponse = await _productionPlanService.GetAll(productionPlanFilterModel);

            BaseReponse response = new BaseReponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Get all production plan sucessfully",
                Data = pageResponse
            };
            return Ok(response);
        }

        [HttpPatch]
        [Route(APIEndPoint.PRODUCTION_PLANS_ID_V1 + APIEndPoint.APPROVE)]
        [SwaggerOperation(Summary = "Approve production plan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Approve production plan successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Approve production plan failed")]
        [Produces("application/json")]
        [Authorize(Roles = "FactoryDirector")]
        public async Task<IActionResult> ApproveProductionPlan([FromRoute] Guid id)
        {
            _currentLoginUserDTO.DecryptAccessToken(Request.Headers["Authorization"]);
            var responseData = await _productionPlanService.Approve(id);
            return Ok(new BaseReponse { StatusCode = 200, Message = "Approve production plan successfully", Data = responseData });
        }

        [HttpPatch]
        [Route(APIEndPoint.PRODUCTION_PLANS_ID_V1 + APIEndPoint.DECLINE)]
        [SwaggerOperation(Summary = "Decline production plan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Decline production plan successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Decline production plan failed")]
        [Produces("application/json")]
        [Authorize(Roles = "FactoryDirector")]
        public async Task<IActionResult> DeclineProductionPlan([FromRoute] Guid id)
        {
            _currentLoginUserDTO.DecryptAccessToken(Request.Headers["Authorization"]);
            var responseData = await _productionPlanService.Decline(id);
            return Ok(new BaseReponse { StatusCode = 200, Message = "Change status of production plan sucessfully", Data = responseData });
        }

        [HttpPatch]
        [Route(APIEndPoint.PRODUCTION_PLANS_ID_V1_START)]
        [SwaggerOperation(Summary = "Start production plan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Start production plan successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid status")]
        [Produces("application/json")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> StartProducitonPlan([FromRoute] Guid id)
        {
            _currentLoginUserDTO.DecryptAccessToken(Request.Headers["Authorization"]);
            var responseData = await _productionPlanService.StartProductionPlan(id);

            return Ok(new BaseReponse { StatusCode = 200, Message = "Start production plan sucessfully", Data = responseData });
        }
    }
}