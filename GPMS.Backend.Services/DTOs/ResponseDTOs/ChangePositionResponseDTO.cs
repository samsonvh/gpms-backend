using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.ResponseDTOs
{
    public class ChangePositionResponseDTO<Model, P> where Model : class where P : Enum
    {
        public Guid Id { get; set; }
        public P Position { get; set; }
    }
}
