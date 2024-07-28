using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan
{
    public class ProductionEstimationInputDTO
    {
        public int Quantity { get; set; }
        public int OverTimeQuantity { get; set; }
        public string? Quarter { get; set; }
        public string? Month { get; set; }
        public int? Batch { get; set; }
        public int? DayNumber {  get; set; }
        public List<ProductionSeriesInputDTO>? ProductionSeries { get; set; }
        = new List<ProductionSeriesInputDTO>();
    }
}
