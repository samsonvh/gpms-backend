using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class CurrentLoginUserDTO
    {
        public Guid StaffId { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
    }
}