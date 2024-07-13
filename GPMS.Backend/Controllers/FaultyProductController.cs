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
    public class FaultyProductController : ControllerBase
    {
        private readonly ILogger<FaultyProductController> _logger;

        public FaultyProductController(ILogger<FaultyProductController> logger)
        {
            _logger = logger;
        }

    }
}