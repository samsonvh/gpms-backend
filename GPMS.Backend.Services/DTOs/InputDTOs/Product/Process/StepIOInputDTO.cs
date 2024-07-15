using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Enums.Types;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Product.Process
{
    public class StepIOInputDTO
    {
        public int? Quantity { get; set; }
        public float? Consumption { get; set; }
        public bool IsProduct { get; set; }
        public ProductionProcessStepIOType Type { get; set; }
        public string? MaterialCode { get; set; }
        public string? SemiFinishedProductCode { get; set; }
    }
}