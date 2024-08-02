using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Services.PageRequests;

namespace GPMS.Backend.Services.Filters
{
    public class ProcessFilterModel : BaseFilterModel
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}