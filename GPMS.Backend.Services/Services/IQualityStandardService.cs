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
    public interface IQualityStandardService
    : IBaseService<QualityStandardInputDTO,CreateUpdateResponseDTO<QualityStandard>,
    QualityStandardListingDTO,QualityStandardDTO>
    {
        Task AddList (List<QualityStandardInputDTO> inputDTOs, Guid specificationId, 
        List<CreateUpdateResponseDTO<Material>> materialCodeList);
    }
}