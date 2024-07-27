using GPMS.Backend.Data.Enums.Times;
using GPMS.Backend.Data.Models.ProductionPlans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class ProductionEstimationDTO
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public int OvertimeQuantity { get; set; }
        public Quarter? Quarter { get; set; }
        public Month? Month { get; set; }
        public int? Batch { get; set; }
        public int? DayNumber { get; set; }
        public List<ProductionSeriesDTO> ProductionSeries { get; set; }
    }
}
