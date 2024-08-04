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
        private readonly CurrentLoginUserDTO _currentLoginUser;

        public ProductController(ILogger<ProductController> logger,
        IProductService productService, IMapper mapper,
        CurrentLoginUserDTO currentLoginUser)
        {
            _logger = logger;
            _productService = productService;
            _mapper = mapper;
            _currentLoginUser = currentLoginUser;
        }
        [HttpPost]
        [Route(APIEndPoint.PRODUCTS_V1)]
        [SwaggerOperation(Summary = "Define product using form")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Define Product Successfully")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Define Product Failed")]
        [Produces("application/json")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DefineProduct([FromBody] ProductInputDTO productInputDTO)
        {
            _currentLoginUser.DecryptAccessToken(Request.Headers["Authorization"]);
            var response = await _productService.Add(productInputDTO);
            return Ok(response);
        }

        [HttpGet]
        [Route(APIEndPoint.PRODUCTS_ID_V1)]
        [SwaggerOperation(Summary = "Get details product")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get Details Product Successfully")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Product not found", typeof(ProductDTO))]
        [Produces("application/json")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            var product = await _productService.Details(id);
            return Ok(product);
        }

        [HttpPatch]
        [Route(APIEndPoint.PRODUCTS_ID_V1)]
        [SwaggerOperation(Summary = "Change status of product")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Change stauts of product successfully", typeof(ProductDTO))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid status")]
        [Produces("application/json")]
        [Authorize("Factory Director")]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            _currentLoginUser.DecryptAccessToken(Request.Headers["Authorization"]);
            var product = await _productService.ChangeStatus(id, status);
            return Ok(product);
        }

        [HttpPost]
        [Route(APIEndPoint.PRODUCTS_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get all product", Description = "Factory director, Production manager can get all product")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all product successfully", typeof(List<ProductListingDTO>))]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllProducts([FromBody] ProductFilterModel productFilterModel)
        {
            DefaultPageResponseListingDTO<ProductListingDTO> pageResponse = await _productService.GetAll(productFilterModel);
            return Ok(pageResponse);
        }

        [HttpGet]
        [Route(APIEndPoint.PRODUCTS_V1 + "/create-production-plan")]
        [SwaggerOperation(Summary = "Get all product for create production plan", Description = "Factory director, Production manager can get all production plan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get all product successfully", typeof(List<CreateProductListingDTO>))]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllProductsForCreateProductionPlan()
        {
            List<CreateProductListingDTO> createProductListingDTOs = await _productService.GetAllProductForCreateProductionPlan();
            return Ok(createProductListingDTOs);
        }

    }
}