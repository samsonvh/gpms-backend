using GPMS.Backend.Data.Enums.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs
{
    public class AccountInputDTO
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public StaffInputDTO StaffInputDTO { get; set; }
    }
}
