using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification
{
    public class SpecificationInputDTO
    {
        public string Size { get; set; }
        public string Color { get; set; }
        public List<MeasurementInputDTO> Measurements { get; set; }
        public List<BOMInputDTO> BOMs { get; set; }
        public List<QualityStandardInputDTO> QualityStandards { get; set; }
    }
}