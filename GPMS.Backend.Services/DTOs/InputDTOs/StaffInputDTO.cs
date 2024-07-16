using GPMS.Backend.Data.Enums.Others;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Staffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs
{
    public class StaffInputDTO
    {
        public string Code { get; set; }
        public string FullName { get; set; }
        public StaffPosition Position { get; set; }
        public Guid? DepartmentId { get; set; }
    }
}
