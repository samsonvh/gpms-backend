using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.ResponseDTOs;

namespace GPMS.Backend.Services.Services
{
    public interface IProductService 
    : IBaseService<ProductInputDTO,CreateUpdateResponseDTO<Product>,ProductListingDTO,ProductDTO>
    {
        Task<CreateUpdateResponseDTO<Product>> Add(ProductInputDTO inputDTO, CurrentLoginUserDTO currentLoginUserDTO);
        Task<ChangeStatusResponseDTO<Product, ProductStatus>> ChangeStatus(Guid id, string productStatus);
    }
}