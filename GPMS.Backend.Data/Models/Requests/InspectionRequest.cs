using GPMS.Backend.Data.Enums.Statuses.Requests;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Results;
using GPMS.Backend.Data.Models.Staffs;

namespace GPMS.Backend.Data.Models.Requests
{
    public class InspectionRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public InspectionRequestStatus Status { get; set; }

        public Guid CreatorId { get; set; }
        public Staff Creator { get; set; }
        public Guid? ReviewerId { get; set; }
        public Staff? Reviewer { get; set; }
        public Guid ProductionSeriesId { get; set; }
        public ProductionSeries ProductionSeries { get; set; }
        public InspectionRequestResult? InspectionRequestResult { get; set; }
    }
}
