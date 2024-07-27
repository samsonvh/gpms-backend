using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class ChildProductionPlanDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime ExpectedStartingDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ActualStartingDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }
}
