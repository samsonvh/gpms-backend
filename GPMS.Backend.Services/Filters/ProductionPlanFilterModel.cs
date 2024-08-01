using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.PageRequests
{
    public class ProductionPlanFilterModel : BaseFilterModel
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public DateTime? ExpectedStartingDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
    }
}
