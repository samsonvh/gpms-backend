using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.LisingDTOs
{
    public class ProductionRequirementListingDTO
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public Guid SpecificationId {get; set;}
    }
}