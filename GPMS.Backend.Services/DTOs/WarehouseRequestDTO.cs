using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class WarehouseRequestDTO
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public Guid? ReviewerId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public WarehouseTicketDTO? WarehouseTicket { get; set; }
    }
}
