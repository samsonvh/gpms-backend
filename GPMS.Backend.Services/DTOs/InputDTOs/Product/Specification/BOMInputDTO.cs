using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification
{
    public class BOMInputDTO
    {
        public float SizeWidth { get; set; }
        public float Consumption { get; set; }
        public string? Description { get; set; }
        public string MaterialCode { get; set; }
    }
}