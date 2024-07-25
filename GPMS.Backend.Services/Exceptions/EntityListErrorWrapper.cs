using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Exceptions
{
    public class EntityListErrorWrapper
    {
        public List<EntityListError> EntityListErrors { get; set; } = new List<EntityListError>();
    }
}