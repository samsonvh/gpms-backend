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
    public class EstimationController : ControllerBase
    {
        private readonly ILogger<EstimationController> _logger;

        public EstimationController(ILogger<EstimationController> logger)
        {
            _logger = logger;
        }

    }
}