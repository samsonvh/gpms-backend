using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.ResponseDTOs;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class ProductService : IProductService
    {
        public Task<CreateUpdateResponseDTO<Product>> Add(ProductInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task AddList(List<ProductInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDTO> Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductListingDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CreateUpdateResponseDTO<Product>> Update(ProductInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<ProductInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }
    }
}