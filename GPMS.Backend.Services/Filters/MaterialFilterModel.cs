using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Services.PageRequests;

namespace GPMS.Backend.Services.Filters
{
    public class MaterialFilterModel : BaseFilterModel
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? ColorCode { get; set; }
        public string? ColorName { get; set; }
    }
}