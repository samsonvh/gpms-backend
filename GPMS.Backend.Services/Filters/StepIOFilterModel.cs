using GPMS.Backend.Services.PageRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Filters
{
    public class StepIOFilterModel : BaseFilterModel
    {
        public bool? IsProduct { get; set; }
        public string? Type { get; set; }
    }
}
