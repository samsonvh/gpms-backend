using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace GPMS.Backend.Controllers
{
    [ApiController]
    public class BillOfMaterialController : ControllerBase
    {
        private readonly ILogger<BillOfMaterialController> _logger;
        private readonly IBillOfMaterialService _billOfMaterialService;

        public BillOfMaterialController(ILogger<BillOfMaterialController> logger, IBillOfMaterialService billOfMaterialService)
        {
            _logger = logger;
            _billOfMaterialService = billOfMaterialService;
        }

        [HttpPost]
        [Route(APIEndPoint.BILL_OF_MATERIALS_OF_SPECIFICATION_ID_V1 + APIEndPoint.FILTER)]
        [SwaggerOperation(Summary = "Get All Bill Of Material Of Specification")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Get All Bill Of Material Sucessfully")]
        [Produces("application/json")]
        /*[Authorize(Roles = "Manager")]*/
        public async Task<IActionResult> GetAllBillOfMaterial([FromRoute] Guid id, [FromBody] BOMFilterModel bOMFilterModel)
        {
            var bom = await _billOfMaterialService.GetAllBomBySpecification(id, bOMFilterModel);
            return Ok(bom);
        }
    }
}