using GPMS.Backend.Services.PageRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Filters
{
    public class ProductionSeriesFilterModel : BaseFilterModel
    {
        public string? Code { get; set; }
        public string? Status { get; set; }
        public int? DayNumber { get; set; }
    }
}
