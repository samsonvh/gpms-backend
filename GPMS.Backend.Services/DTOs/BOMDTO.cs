using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;

namespace GPMS.Backend.Services.DTOs
{
    public class BOMDTO
    {
        public Guid Id { get; set; }
        public float SizeWidth { get; set; }
        public float Consumption { get; set; }
        public string? Description { get; set; }
    }
}