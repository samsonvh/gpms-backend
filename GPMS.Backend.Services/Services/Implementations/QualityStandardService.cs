using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class QualityStandardService : IQualityStandardService
    {
        private readonly IGenericRepository<QualityStandard> _qualityStandardRepository;
        private readonly IValidator<QualityStandardInputDTO> _qualityStandardValidator;
        private readonly IMapper _mapper;
        public QualityStandardService(
            IGenericRepository<QualityStandard> qualityStandardRepository,
            IValidator<QualityStandardInputDTO> qualityStandardValidator,
            IMapper mapper
            )
        {
            _qualityStandardRepository = qualityStandardRepository;
            _qualityStandardValidator = qualityStandardValidator;
            _mapper = mapper;
        }

        public Task<CreateUpdateResponseDTO<QualityStandard>> Add(QualityStandardInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task AddList(List<QualityStandardInputDTO> inputDTOs, Guid? parentEntityId = null)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<QualityStandardInputDTO> inputDTOs, Guid specificationId, List<CreateUpdateResponseDTO<Material>> materialCodeList)
        {
            ValidateQualityStandardInputDTOList(inputDTOs);
            ValidateMaterialCodeInQualityStandardWithMaterialCodeList(inputDTOs,materialCodeList);
            foreach (QualityStandardInputDTO qualityStandardInputDTO in inputDTOs)
            {
                QualityStandard qualityStandard = _mapper.Map<QualityStandard>(qualityStandardInputDTO);
                qualityStandard.ProductSpecificationId = specificationId;
                qualityStandard.MaterialId = materialCodeList
                .First(materialCode => materialCode.Code.Equals(qualityStandardInputDTO.MaterialCode))
                .Id;
                _qualityStandardRepository.Add(qualityStandard);
            }
        }

        public Task<QualityStandardDTO> Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<QualityStandardListingDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CreateUpdateResponseDTO<QualityStandard>> Update(QualityStandardInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<QualityStandardInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }
        private void ValidateQualityStandardInputDTOList(List<QualityStandardInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (QualityStandardInputDTO inputDTO in inputDTOs)
            {
                FluentValidation.Results.ValidationResult validationResult = _qualityStandardValidator.Validate(inputDTO);
                if (!validationResult.IsValid)
                {
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        errors.Add(new FormError
                        {
                            ErrorMessage = validationFailure.ErrorMessage,
                            Property = validationFailure.PropertyName
                        });
                    }
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Quality standard list invalid", errors);
            }
        }
        private void ValidateMaterialCodeInQualityStandardWithMaterialCodeList(List<QualityStandardInputDTO> inputDTOs,
        List<CreateUpdateResponseDTO<Material>> materialCodeList)
        {
            List<FormError> errors = new List<FormError>();
            foreach (QualityStandardInputDTO qualityStandardInputDTO in inputDTOs)
            {
                if (materialCodeList.FirstOrDefault(materialCode => materialCode.Code.Equals(qualityStandardInputDTO.MaterialCode)) == null)
                {
                    errors.Add(new FormError
                    {
                        Property = typeof(QualityStandardInputDTO).GetProperty("MaterialCode").Name,
                        ErrorMessage = $"Material code: {qualityStandardInputDTO.MaterialCode} of" +
                        "quality standard : {qualityStandardInputDTO.Name} is not valid"
                    });
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Material code in quality standard list invalid", errors);
            }
        }
    }
}