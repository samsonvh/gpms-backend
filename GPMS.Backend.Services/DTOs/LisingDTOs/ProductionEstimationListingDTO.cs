using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.LisingDTOs
{
    public class ProductionEstimationListingDTO
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public int OverTimeQuantity { get; set; }
        public int? Quarter {  get; set; }
        public int? Month { get; set; }
        public int? Batch {  get; set; }
        public int? DayNumber { get; set; }
    }
}
