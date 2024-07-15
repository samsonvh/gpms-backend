using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.ResponseDTOs
{
    public class ChangeStatusResponseDTO<Model, S> where Model : class where S : Enum
    {
        public Guid Id { get; set; }
        public S Status { get; set; }
    }
}