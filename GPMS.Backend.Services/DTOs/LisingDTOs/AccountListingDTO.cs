using GPMS.Backend.Data.Enums.Others;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.LisingDTOs
{
    public class AccountListingDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string Poition { get; set; }
        public string Status { get; set; }
    }
}
