using GPMS.Backend.Data.Enums.Types;
using GPMS.Backend.Data.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Data.Models.Products.ProductionProcesses
{
    public class ProductionProcessStepIO
    {
        public Guid Id { get; set; }
        public int? Quantity { get; set; }
        public float? Consumption { get; set; }
        public bool IsProduct { get; set; }
        public ProductionProcessStepIOType Type { get; set; }

        public Guid SemiFinishedProductId { get; set; }
        public SemiFinishedProduct SemiFinishedProduct { get; set; }
        public Guid? MaterialId { get; set; }
        public Material? Material { get; set; }
        public Guid ProductionProcessStepId { get; set; }
        public ProductionProcessStep ProductionProcessStep { get; set; }

        public ICollection<ProductionProcessStepIOResult> ProductionProcessStepIOResults { get; set; } = new List<ProductionProcessStepIOResult>();
    }
}
