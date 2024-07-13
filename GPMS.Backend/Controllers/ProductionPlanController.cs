using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class ProductionPlanController : ControllerBase
    {
        private readonly ILogger<ProductionPlanController> _logger;

        public ProductionPlanController(ILogger<ProductionPlanController> logger)
        {
            _logger = logger;
        }

    }
}