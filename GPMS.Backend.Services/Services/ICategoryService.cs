using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;

namespace GPMS.Backend.Services.Services
{
    public interface ICategoryService 
    {
        Task<CreateUpdateResponseDTO<Category>> Add(CategoryInputDTO inputDTO);
        Task<CategoryDTO> DetailsByName (string name);
        Task<CategoryDTO> Details(Guid id);
        Task<DefaultPageResponseListingDTO<CategoryDTO>> GetAll(CategoryFilterModel categoryFilterModel);
    }
}