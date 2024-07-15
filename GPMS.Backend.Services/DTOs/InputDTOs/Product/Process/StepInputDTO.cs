using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Product.Process
{
    public class StepInputDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public float StandardTime { get; set; }
        public float OutputPerHour { get; set; }
        public string? Description { get; set; }
        public List<StepIOInputDTO> StepIOs { get; set; }
    }
}