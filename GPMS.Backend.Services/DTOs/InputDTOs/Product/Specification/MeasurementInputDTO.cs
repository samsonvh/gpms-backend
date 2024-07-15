using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification
{
    public class MeasurementInputDTO
    {
        public string Name { get; set; }
        public float Measure { get; set; }
        public string Unit { get; set; }
    }
}