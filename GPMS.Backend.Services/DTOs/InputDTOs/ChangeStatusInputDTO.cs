using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs
{
    public class ChangeStatusInputDTO
    {
        public string Status { get; set; }
        public string? Description { get; set; }
    }
}
