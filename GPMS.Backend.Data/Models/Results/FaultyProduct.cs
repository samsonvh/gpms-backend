using GPMS.Backend.Data.Models.Products.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Data.Models.Results
{
    public class FaultyProduct
    {
        public Guid Id { get; set; }
        public int ProductOrderNumber { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid InspectionRequestResultId { get; set; }
        public InspectionRequestResult InspectionRequestResult { get; set; }
        public Guid SpecificationId { get; set; }
        public ProductSpecification Specification { get; set; }

        public ICollection<ProductFault> ProductFaults { get; set; } = new List<ProductFault>();
    }
}
