using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Data.Enums.Statuses.ProductionPlans
{
    public enum ProductionSeriesStatus
    {
        Pending,
        InProduction,
        InInspection,
        InPackaging,
        Done,
    }
}
