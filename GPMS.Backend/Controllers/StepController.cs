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
    public class StepController : ControllerBase
    {
        private readonly ILogger<StepController> _logger;

        public StepController(ILogger<StepController> logger)
        {
            _logger = logger;
        }

    }
}