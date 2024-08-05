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
    public interface IBillOfMaterialService
    : IBaseService<BOMInputDTO, CreateUpdateResponseDTO<BillOfMaterial>,
    BOMListingDTO,BOMDTO, BaseFilterModel>
    {
        Task AddList(List<BOMInputDTO> inputDTOs,Guid specificationId);
        Task<DefaultPageResponseListingDTO<BOMListingDTO>> GetAllBomBySpecification(Guid specificationId, BOMFilterModel bOMFilterModel);
    }
}