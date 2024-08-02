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
    public class SemiFinishedProductsController : ControllerBase
    {
        private readonly ISemiFinishedProductService _semiFinishedProductService;
        private readonly ILogger<SemiFinishedProductsController> _logger;

        public SemiFinishedProductsController(ILogger<SemiFinishedProductsController> logger, ISemiFinishedProductService semiFinishedProductService)
        {
            _logger = logger;
            _semiFinishedProductService = semiFinishedProductService;
        }

        [HttpPost]
        [Route(APIEndPoint.SEMI_FINISHED_PRODUCT_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all semi finished products")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all semi finished product successfully", typeof(DefaultPageResponseListingDTO<SemiFinishedProductListingDTO>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "SemiFinished Product not found")]
        [Produces("application/json")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllSemiFinishedProduct([FromBody] SemiFinishedProductFilterModel semiFinishedProductFilterModel)
        {
            var response = await _semiFinishedProductService.GetAll(semiFinishedProductFilterModel);
            return Ok(response);
        }

        [HttpPost]
        [Route(APIEndPoint.SEMI_FINISHED_PRODUCT_OF_PRODUCT_ID_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all semifinished products")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all semi finished product successfully", typeof(DefaultPageResponseListingDTO<SemiFinishedProductListingDTO>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Semi Finished Product not found")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllSemiFinishedProductOfProduct([FromRoute] Guid id, [FromBody] SemiFinishedProductFilterModel semiFinishedProductFilterModel)
        {
            var result = await _semiFinishedProductService.GetAllSemiOfProduct(id, semiFinishedProductFilterModel);
            BaseReponse response = new BaseReponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Get all processes of product",
                Data = result
            };
            return Ok(response);
        }
    }
}
