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
    public class InspectionRequestController : ControllerBase
    {
        private readonly ILogger<InspectionRequestController> _logger;

        public InspectionRequestController(ILogger<InspectionRequestController> logger)
        {
            _logger = logger;
        }

    }
}