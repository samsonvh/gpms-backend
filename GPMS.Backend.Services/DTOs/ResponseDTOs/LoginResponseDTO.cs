using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.ResponseDTOs
{
    public class LoginResponseDTO
    {
        public AccountResponseDTO Account { get; set; }
        public string AccessToken { get; set; }
    }
}