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
    public class BillOfMaterialController : ControllerBase
    {
        private readonly ILogger<BillOfMaterialController> _logger;

        public BillOfMaterialController(ILogger<BillOfMaterialController> logger)
        {
            _logger = logger;
        }

    }
}