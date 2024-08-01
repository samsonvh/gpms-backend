using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Services.Filters;

namespace GPMS.Backend.Services.PageRequests
{
    public class BaseFilterModel 
    {
        public string? OrderBy { get; set; }
        public bool IsAscending { get; set; } = true;
        public PaginationRequestModel? Pagination { get; set; }
    }
}