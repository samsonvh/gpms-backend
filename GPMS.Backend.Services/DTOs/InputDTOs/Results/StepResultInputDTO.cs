using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Results
{
    public class StepResultInputDTO
    {
        public Guid StepId { get; set; }
        public string Description { get; set; }

        public List<InputOutputResultInputDTO> inputOutputResults { get; set; }
    }
}