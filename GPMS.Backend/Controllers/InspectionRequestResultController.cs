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
    public class InspectionRequestResultController : ControllerBase
    {
        private readonly ILogger<InspectionRequestResultController> _logger;

        public InspectionRequestResultController(ILogger<InspectionRequestResultController> logger)
        {
            _logger = logger;
        }

    }
}