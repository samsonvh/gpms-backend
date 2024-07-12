using GPMS.Backend.Data.Enums.Statuses.Requests;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Models.Warehouses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Data.Models.Requests
{
    public class WarehouseRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public WarehouseRequestStatus Status { get; set; }

        public Guid CreatorId { get; set; }
        public Staff Creator { get; set; }
        public Guid? ReviewerId { get; set; }
        public Staff? Reviewer { get; set; }
        public Guid ProductionRequirementId { get; set; }
        public ProductionRequirement ProductionRequirement { get; set; }
        public WarehouseTicket? WarehouseTicket { get; set; }
    }
}
