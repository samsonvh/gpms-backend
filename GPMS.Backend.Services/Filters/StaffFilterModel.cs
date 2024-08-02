using GPMS.Backend.Services.PageRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Filters
{
    public class StaffFilterModel : BaseFilterModel
    {
        public string? Code { get; set; }
        public string? Position { get; set; }
        public string? Status { get; set; }
    }
}
