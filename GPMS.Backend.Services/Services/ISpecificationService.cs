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
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.PageRequests;

namespace GPMS.Backend.Services.Services
{
    public interface ISpecificationService
    : IBaseService<SpecificationInputDTO, CreateUpdateResponseDTO<ProductSpecification>,
        SpecificationListingDTO, SpecificationDTO, SpecificationFilterModel>
    {
        Task AddList(List<SpecificationInputDTO> inputDTOs, Guid productId, string sizes, string colors);
        /*Task<List<CreateProductSpecificationListingDTO>> GetSpecificationByProductId(Guid productId);*/
        Task<DefaultPageResponseListingDTO<SpecificationListingDTO>> GetAll(SpecificationFilterModel specificationFilterModel);

        Task<DefaultPageResponseListingDTO<SpecificationListingDTO>> GetAllSpcificationByProductId(Guid productId, SpecificationFilterModel specificationFilterModel);

    }
}