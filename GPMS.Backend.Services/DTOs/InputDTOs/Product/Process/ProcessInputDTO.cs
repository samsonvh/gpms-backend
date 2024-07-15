using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Product.Process
{
    public class ProcessInputDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public string? Description { get; set; }
        public List<StepInputDTO> Steps { get; set; }
    }
}