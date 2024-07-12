using GPMS.Backend.Data.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Data.Models.Products.Specifications
{
    public class Measurement
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Measure { get; set; }
        public string Unit { get; set; }

        public Guid ProductSpecificationId { get; set; }
        public ProductSpecification ProductSpecification { get; set; }

        public ICollection<ProductFault> ProductFaults { get; set; } = new List<ProductFault>();
    }
}
