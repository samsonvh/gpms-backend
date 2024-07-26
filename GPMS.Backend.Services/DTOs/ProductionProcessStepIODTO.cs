using GPMS.Backend.Data.Enums.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class ProductionProcessStepIODTO
    {
        public Guid Id { get; set; }
        public int? Quantity { get; set; }
        public float? Consumption { get; set; }
        public bool IsProduct { get; set; }
        public string Type { get; set; }
    }
}
