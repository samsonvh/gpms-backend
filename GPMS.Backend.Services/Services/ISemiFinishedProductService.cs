using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;

namespace GPMS.Backend.Services.Services
{
    public interface ISemiFinishedProductService 
    : IBaseService<SemiFinishedProductInputDTO,CreateUpdateResponseDTO<SemiFinishedProduct>,
    SemiFinishedProductListingDTO,SemiFinishedProductDTO, SemiFinishedProductFilterModel>
    {
        Task<List<CreateUpdateResponseDTO<SemiFinishedProduct>>> AddList(List<SemiFinishedProductInputDTO> inputDTOs, Guid productId);
        Task<DefaultPageResponseListingDTO<SemiFinishedProductListingDTO>> GetAllSemiOfProduct(Guid productId, SemiFinishedProductFilterModel semiFinishedProductFilterModel);
    }
}