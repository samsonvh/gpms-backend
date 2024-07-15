using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Enums.Others;
using GPMS.Backend.Data.Enums.Statuses.Staffs;

namespace GPMS.Backend.Services.DTOs.ResponseDTOs
{
    public class LoginResponseDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public StaffPosition Position { get; set; }
        public AccountStatus Status { get; set; }
        public string Token { get; set; }
    }
}