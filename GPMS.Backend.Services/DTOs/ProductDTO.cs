using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Enums.Statuses.Products;

namespace GPMS.Backend.Services.DTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<string> Sizes { get; set; }
        public List<string> Colors { get; set; }
        public List<string>? ImageURLs { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public CategoryDTO Category { get; set; }
        public string CreatorName { get; set; }
        public string ReviewerName { get; set; }

        public List<SemiFinishedProductDTO> SemiFinishedProducts { get; set; }
        public List<SpecificationDTO> Specifications { get; set; }
        public List<ProcessDTO> Processes { get; set; }
    }
}