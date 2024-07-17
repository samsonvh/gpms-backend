using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class MeasurementDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Measure { get; set; }
        public string Unit { get; set; }
    }
}