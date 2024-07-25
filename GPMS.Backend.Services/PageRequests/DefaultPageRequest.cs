using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.PageRequests
{
    public class DefaultPageRequest
    {
        public string? OrderBy { get; set; } = null;
        public bool IsAscending { get; set; } = true;
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}