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
    public class RequirementController : ControllerBase
    {
        private readonly ILogger<RequirementController> _logger;

        public RequirementController(ILogger<RequirementController> logger)
        {
            _logger = logger;
        }

    }
}