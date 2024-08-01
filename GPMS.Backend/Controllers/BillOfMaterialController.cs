using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

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
        // [HttpPost]
        // [Route(APIEndPoint.BILL_OF_MATERIALS_OF_SPECIFICATION_ID_V1 + APIEndPoint.FILTER)]
        // [SwaggerOperation(Summary = "Get All Bill Of Material")]
        // [SwaggerResponse((int)HttpStatusCode.OK, "Bill Of Material List")]
        // [Produces("application/json")]
        // [Authorize(Roles = "Manager")]
        // public async Task<IActionResult> GetAllBillOfMaterial (BillOf)
        // {
        //     return Ok();
        // }
    }
}