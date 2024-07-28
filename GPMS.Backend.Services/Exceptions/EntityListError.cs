using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Exceptions
{
    public class EntityListError
    {
        public string Entity { get; set; }
        public List<FormError> Errors { get; set; } = new List<FormError>();
    }
    
}