using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Services.PageRequests;

namespace GPMS.Backend.Services.Filters
{
    public class AccountFilterModel : BaseFilterModel
    {
        public string? Code { get; set; }
        public string? Email { get; set; }
        public string? AccountStatus { get; set; }
    }
}