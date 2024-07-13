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
    public class WarehouseRequestController : ControllerBase
    {
        private readonly ILogger<WarehouseRequestController> _logger;

        public WarehouseRequestController(ILogger<WarehouseRequestController> logger)
        {
            _logger = logger;
        }

    }
}