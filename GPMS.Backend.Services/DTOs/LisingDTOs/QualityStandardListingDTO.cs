using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.LisingDTOs
{
    public class QualityStandardListingDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<string> ImageURL { get; set; } = new List<string>();
    }
}