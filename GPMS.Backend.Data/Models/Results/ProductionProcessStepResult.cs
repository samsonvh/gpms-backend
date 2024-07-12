using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Staffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Data.Models.Results
{
    public class ProductionProcessStepResult
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public ProductionProcessStepResultStatus Status { get; set; }

        public Guid CreatorId { get; set; }
        public Staff Creator { get; set; }
        public Guid ProductionSeriesId { get; set; }
        public ProductionSeries ProductionSeries { get; set; }
        public Guid? InspectionRequestResultId { get; set; }
        public InspectionRequestResult? InspectionRequestResult { get; set; }
        public Guid? ProductionProcessStepId { get; set; }
        public ProductionProcessStep? ProductionProcessStep { get; set; }

        public ICollection<ProductionProcessStepIOResult> StepIOResults { get; set; } = new List<ProductionProcessStepIOResult>();
    }
}
