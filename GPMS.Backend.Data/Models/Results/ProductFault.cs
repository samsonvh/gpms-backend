using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Products.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Data.Models.Results
{
    public class ProductFault
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }

        public Guid FaultyProductId { get; set; }
        public FaultyProduct FaultyProduct { get; set; }
        public Guid QualityStandardId { get; set; }
        public QualityStandard QualityStandard { get; set; }
        public Guid ProductionProcessStepId { get; set; }
        public ProductionProcessStep ProductionProcessStep { get; set; }
        public Guid? ProductMeasurementId { get; set; }
        public Measurement? ProductMeasurement { get; set; }

    }
}
