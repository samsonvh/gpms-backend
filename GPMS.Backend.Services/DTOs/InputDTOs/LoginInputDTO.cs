using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs
{
    public class LoginInputDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}