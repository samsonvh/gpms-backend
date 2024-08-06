using GPMS.Backend.Services.PageRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Filters
{
    public class ProductionEstimationFilterModel : BaseFilterModel
    {
        public int? Quarter { get; set; }
        public int? Month { get; set; }
        public int? Batch { get; set; }
        public int? DayNumber { get; set; }
    }
}
