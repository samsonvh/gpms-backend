using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Services;
using GPMS.Backend.Services.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route(APIEndPoint.CATEGORY_V1)]
        [SwaggerOperation(Summary = "Create Category")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Create Category Successfully")]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateCatgory(CategoryInputDTO categoryInputDTO)
        {
            return Ok(await _categoryService.Add(categoryInputDTO));
        }

        [HttpGet]
        [Route(APIEndPoint.CATEGORY_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get All Category")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Category List", typeof(CategoryDTO))]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllCategory([FromBody] CategoryFilterModel categoryFilterModel)
        {
            var data = await _categoryService.GetAll(categoryFilterModel);
            return Ok(data);
        }

        [HttpGet]
        [Route(APIEndPoint.CATEGORY_ID_V1)]
        [SwaggerOperation(Summary = "Get details of category")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get details of category successfully", typeof(CategoryDTO))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Category not found")]
        [Produces("application/json")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            var category = await _categoryService.Details(id);
            return Ok(category);
        }
    }
}