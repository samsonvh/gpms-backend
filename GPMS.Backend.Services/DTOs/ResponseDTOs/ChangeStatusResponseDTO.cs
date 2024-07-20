using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.ResponseDTOs
{
    public class ChangeStatusResponseDTO<Model, S> where Model : class 
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
}