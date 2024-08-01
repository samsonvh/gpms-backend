using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class SpecificationController : ControllerBase
    {
        private readonly ILogger<SpecificationController> _logger;
        private readonly ISpecificationService _specificationService;

        public SpecificationController(ILogger<SpecificationController> logger, ISpecificationService specificationService)
        {
            _logger = logger;
            _specificationService = specificationService;
        }

        [HttpGet]
        [Route(APIEndPoint.SPECIFICATIONS_OF_PRODUCT_ID_V1)]
        [SwaggerOperation(Summary = "Get all specification for create production plan", Description = "Factory director, Production manager can get all specification for create production plan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all specification successfully", typeof(List<CreateProductSpecificationListingDTO>))]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllProductsForCreateProductionPlan([FromRoute] Guid id)
        {
            List<CreateProductSpecificationListingDTO> createProductSpecificationListingDTOs = 
                await _specificationService.GetSpecificationByProductId(id);

            BaseReponse response = new BaseReponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Get all specification successfully",
                Data = createProductSpecificationListingDTOs
            };
            return Ok(response);
        }

    }
}