using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.LisingDTOs
{
    public class IOResultListingDTO
    {
        public Guid Id { get; set; }
        public float Consumption {  get; set; }
        public int Quantity { get; set; }
    }
}
