using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;

namespace GPMS.Backend.Services.Services
{
    public interface IMeasurementService 
    : IBaseService<MeasurementInputDTO,CreateUpdateResponseDTO<Measurement>,
    MeasurementListingDTO,MeasurementDTO,MeasurementFilterModel>
    {
        Task<DefaultPageResponseListingDTO<MeasurementListingDTO>> GetAllMeasurementsByProductId(Guid specificationId, MeasurementFilterModel measurementFilterModel);
    }
}