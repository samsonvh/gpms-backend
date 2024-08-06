using GPMS.Backend.Data.Models.Products.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.DTOs
{
    public class SpecificationDTO
    {
        public Guid Id { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int InventoryQuantity { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public List<string>? ImageURLs { get; set; }
        public List<MeasurementDTO> Measurements { get; set; }
        public List<BOMDTO> BillOfMaterials { get; set; }
        public List<QualityStandardDTO> QualityStandards { get; set; }
    }
}