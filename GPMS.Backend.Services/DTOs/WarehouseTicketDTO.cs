using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class WarehouseTicketDTO
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid ProductSpeccificationId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
