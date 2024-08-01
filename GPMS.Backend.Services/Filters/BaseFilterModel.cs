using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Services.Filters;

namespace GPMS.Backend.Services.PageRequests
{
    public class BaseFilterModel : PaginationRequestModel
    {
        public string? OrderBy { get; set; } = null;
        public bool IsAscending { get; set; } = true;
    }
}