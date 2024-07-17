using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs.InputDTOs.Product
{
    public class MaterialInputDTO
    {
        //Material Input Field
        public string Code { get; set; }
        public string? Name { get; set; }
        public string? ConsumptionUnit { get; set; }
        public string? SizeWidthUnit { get; set; }
        public string? ColorCode { get; set; }
        public string? ColorName { get; set; }
        public string? Description { get; set; }
        public bool IsNew { get; set; }
    }
}