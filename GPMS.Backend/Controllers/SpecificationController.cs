using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;
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

        [HttpPost]
        [Route(APIEndPoint.SPECIFICATIONS_OF_PRODUCT_ID_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all specification by product", Description = "Factory director, Production manager can get all specification for create production plan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all specification successfully", typeof(DefaultPageResponseListingDTO<SpecificationListingDTO>))]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllSpecificationsByProduct([FromRoute] Guid id, [FromBody] SpecificationFilterModel specificationFilterModel)
        {
            var specficiations = await _specificationService.GetAllSpcificationByProductId(id, specificationFilterModel);
            return Ok(specficiations);
        }

        [HttpPost]
        [Route(APIEndPoint.SPECIFICATIONS_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all specification ", Description = "Factory director, Production manager can get all specification")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all specification successfully", typeof(DefaultPageResponseListingDTO<SpecificationListingDTO>))]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllSpecifications([FromBody] SpecificationFilterModel specificationFilterModel)
        {
            var pageResponses = await _specificationService.GetAll(specificationFilterModel);
            return Ok(pageResponses);
        }

        [HttpPost]
        [Route(APIEndPoint.SPECIFICATIONS_ID_V1)]
        [SwaggerOperation(Summary = "Get specification Id ", Description = "Factory director, Production manager can get all specification")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all specification successfully", typeof(DefaultPageResponseListingDTO<SpecificationListingDTO>))]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetDetailSpecification([FromRoute] Guid id)
        {
            var pageResponses = await _specificationService.Details(id);
            return Ok(pageResponses);
        }

        [HttpPost]
        [Route(APIEndPoint.SPECIFICATIONS_V1 + APIEndPoint.IMAGE)]
        [SwaggerOperation(Summary = "Upload Image For Product Specification", Description = "Production manager upload image for product specification")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Upload Image For Product Specification successfully")]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UploadImageForProductSpecification(ImageSpecificationInputDTO inputDTO)
        {
            var response = await _specificationService.UploadImages(inputDTO);
            return Ok(response);
        }
    }
}