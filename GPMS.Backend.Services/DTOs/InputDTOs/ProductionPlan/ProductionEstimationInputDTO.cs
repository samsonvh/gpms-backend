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
        public int? Quarter { get; set; }
        public int? Month { get; set; }
        public int? Batch { get; set; }
        public int? Day { get; set; }
        public List<ProductionSeriesInputDTO> ProductionSeriesInputDTOs { get; set; }
    }
}
