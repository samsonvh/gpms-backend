using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.Product.InputDTOs.Product
{
    public class CategoryInputDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }
}