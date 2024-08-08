using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.LisingDTOs
{
    public class StepIOForStepResultListingDTOWrapper
    {
        public List<StepIOForStepResultListingDTO> Inputs { get; set; }
        public List<StepIOForStepResultListingDTO> Outputs { get; set; }
    }
}