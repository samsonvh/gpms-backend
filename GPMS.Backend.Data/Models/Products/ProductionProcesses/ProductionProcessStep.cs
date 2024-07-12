using GPMS.Backend.Data.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Data.Models.Products.ProductionProcesses
{
    public class ProductionProcessStep
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public float StandardTime { get; set; }
        public float OutputPerHour { get; set; }
        public string? Description { get; set; }

        public Guid ProductionProcessId { get; set; }
        public ProductProductionProcess ProductionProcess { get; set; }

        public ICollection<ProductFault> ProductFaults { get; set; } = new List<ProductFault>();
        public ICollection<ProductionProcessStepResult> ProductionProcessStepResults { get; set; } = new List<ProductionProcessStepResult>();
        public ICollection<ProductionProcessStepIO> ProductionProcessStepIOs { get; set; } = new List<ProductionProcessStepIO>();
    }
}
