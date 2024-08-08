using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Results
{
    public class InputOutputResultInputDTO
    {
        public Guid StepInputOutputId { get; set; }
        public float? Consumption { get; set; }
        public int? Quantity { get; set; }
    }
}