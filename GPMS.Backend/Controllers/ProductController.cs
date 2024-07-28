using System.Net;
using GPMS.Backend.Data.Enums.Statuses.Products;
using AutoMapper;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.PageRequests;
using GPMS.Backend.Services.Services;
using GPMS.Backend.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(ILogger<ProductController> logger, 
        IProductService productService, IMapper mapper)
        {
            _logger = logger;
            _productService = productService;
            _mapper = mapper;
        }
        [HttpPost]
        [Route(APIEndPoint.PRODUCTS_V1)]
        [SwaggerOperation(Summary = "Define product using form")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Define Product Successfully")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Define Product Failed", typeof(List<EntityListError>))]
        [Produces("application/json")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DefineProduct([FromForm] ProductInputDTO productInputDTO)
        {
            CurrentLoginUserDTO currentLoginUserDTO = JWTUtils.DecryptAccessToken(Request.Headers["Authorization"]);
            CreateUpdateResponseDTO<Product> result = await _productService.Add(productInputDTO, currentLoginUserDTO);
            BaseReponse baseReponse = new BaseReponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Define Product Successfully",
                Data = result
            };
            return Ok(baseReponse);
        }

        [HttpGet]
        [Route(APIEndPoint.PRODUCTS_ID_V1)]
        [SwaggerOperation(Summary = "Get details product")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get Details Product Successfully")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Product not found", typeof(BaseReponse))]
        [Produces("application/json")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            var product = await _productService.Details(id);
            return Ok(new BaseReponse { StatusCode = 200, Message = "Get details of product sucessfully", Data = product });
        }
        [HttpPatch]
        [Route(APIEndPoint.PRODUCTS_ID_V1)]
        [SwaggerOperation(Summary = "Change status of product")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Change stauts of product successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid status")]
        [Produces("application/json")]
        [Authorize("Factory Director")]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            var product = await _productService.ChangeStatus(id, status);
            var responseData = new ChangeStatusResponseDTO<Product, ProductStatus>
            {
                Id = product.Id,
                Status = product.Status
            };

            return Ok(new BaseReponse { StatusCode = 200, Message = "Change status of product sucessfully", Data = responseData });
        }
        [HttpGet]
        [Route(APIEndPoint.PRODUCTS_V1)]
        [SwaggerOperation(Summary = "Get all product",Description = "Factory director, Production manager can get all product")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all product successfully", typeof(List<ProductListingDTO>))]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllProducts([FromQuery]ProductPageRequest productPageRequest)
        {
            DefaultPageResponseListingDTO<ProductListingDTO> pageResponse = await _productService.GetAll(productPageRequest);

            BaseReponse response = new BaseReponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Get all product",
                Data = pageResponse
            };
            return Ok(response);
        }

    }
}