using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Staffs;

namespace GPMS.Backend.Data.Models.Results
{
    public class InspectionRequestResult
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int FaultyQuantity { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid CreatorId { get; set; }
        public Staff Creator { get; set; }
        public Guid InspectionRequestId { get; set; }
        public InspectionRequest InspectionRequest { get; set; }
        public ProductionProcessStepResult ProductionProcessStepResult { get; set; }

        public ICollection<FaultyProduct> FaultyProducts { get; set; } = new List<FaultyProduct>();
    }
}
