using GPMS.Backend.Data.Enums.Statuses.Staffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.LisingDTOs
{
    public class StaffListingDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string? Department { get; set; }
        public string Status { get; set; }
    }
}
