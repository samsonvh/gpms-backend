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
    public class SpecificationController : ControllerBase
    {
        private readonly ILogger<SpecificationController> _logger;

        public SpecificationController(ILogger<SpecificationController> logger)
        {
            _logger = logger;
        }

    }
}