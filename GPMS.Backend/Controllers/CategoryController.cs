using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
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

        [HttpGet]
        [Route(APIEndPoint.CATEGORY_V1)]
        [SwaggerOperation(Summary = "Get All Category")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Category List")]
        [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllCategory ()
        {
            var data = await _categoryService.GetAll();
            BaseReponse response = new BaseReponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Category List",
                Data = data
            };
            return Ok(response);
        }

        [HttpGet]
        [Route(APIEndPoint.CATEGORY_ID_V1)]
        [SwaggerOperation(Summary = "Get details of category")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get details of category successfully", typeof(BaseReponse))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Category not found")]
        [Produces("application/json")]
        public async Task<IActionResult> Details([FromRoute] Guid id)
        {
            var category = await _categoryService.Details(id);
            return Ok(new BaseReponse { StatusCode = 200, Message = "Get details of category sucessfully", Data = category });
        }
    }
}