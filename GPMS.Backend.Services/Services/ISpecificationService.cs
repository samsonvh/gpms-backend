using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;

namespace GPMS.Backend.Services.Services
{
    public interface ISpecificationService
    : IBaseService<SpecificationInputDTO, CreateUpdateResponseDTO<ProductSpecification>,
        SpecificationListingDTO, SpecificationDTO>
    {
        Task AddList(List<SpecificationInputDTO> inputDTOs, Guid productId, 
        List<CreateUpdateResponseDTO<Material>> materialCodeList, string sizes, string colors);
    }
}