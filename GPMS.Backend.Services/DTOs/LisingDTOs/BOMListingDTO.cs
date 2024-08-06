using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.LisingDTOs
{
    public class BOMListingDTO
    {
        public Guid Id { get; set; }
        public float sizeWidth { get; set; }
        public float Consumption { get; set; }
        public MaterialListingDTO Material {  get; set; }
    }
}