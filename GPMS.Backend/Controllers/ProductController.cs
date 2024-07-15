using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }
        // [HttpPost]
        // [Route(APIEndPoint.AUTHENTICATION_CREDENTIALS_V1)]
        // [SwaggerOperation(Summary = "Login to system using email and password")]
        // [SwaggerResponse((int)HttpStatusCode.OK, "Login Successfully", typeof(LoginResponseDTO))]
        // [SwaggerResponse((int)HttpStatusCode.Unauthorized,"Login Failed" )]
        // [Produces("application/json")]
    }
}