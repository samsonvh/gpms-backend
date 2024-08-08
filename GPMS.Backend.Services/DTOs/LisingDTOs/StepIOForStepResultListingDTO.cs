using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Enums.Types;

namespace GPMS.Backend.Services.DTOs.LisingDTOs
{
    public class StepIOForStepResultListingDTO
    {
        public Guid Id { get; set; }
        public int? Quantity { get; set; }
        public float? Consumption { get; set; }
        public bool IsProduct { get; set; }
        public ProductionProcessStepIOType Type { get; set; }

        public int? QuantityForTotalProductInSeres { get; set; }
        public int? ConsumptionForTotalProductInSeres { get; set; }
    }
}