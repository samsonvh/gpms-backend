using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Services;
using GPMS.Backend.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
    }
}