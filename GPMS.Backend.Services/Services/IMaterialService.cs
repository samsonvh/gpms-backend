using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;

namespace GPMS.Backend.Services.Services
{
    public interface IMaterialService
    : IBaseService<MaterialInputDTO, CreateUpdateResponseDTO<Material>,
    MaterialListingDTO,MaterialDTO, MaterialFilterModel>
    {
        Task<List<CreateUpdateResponseDTO<Material>>> AddList (List<MaterialInputDTO> inputDTOs); 
        Task<MaterialDTO> Add(MaterialInputDTO inputDTO);
    }
}