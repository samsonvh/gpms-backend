using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class MaterialService : IMaterialService
    {
        private readonly IGenericRepository<Material> _materialRepository;
        private readonly IValidator<MaterialInputDTO> _materialValidator;
        private readonly IMapper _mapper;

        public MaterialService(
            IGenericRepository<Material> materialRepository,
            IValidator<MaterialInputDTO> materialValidator,
            IMapper mapper)
        {
            _materialRepository = materialRepository;
            _materialValidator = materialValidator;
            _mapper = mapper;
        }

        public Task<CreateUpdateResponseDTO<Material>> Add(MaterialInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<MaterialInputDTO> inputDTOs, Guid? parentEntityId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CreateUpdateResponseDTO<Material>>> AddList(List<MaterialInputDTO> inputDTOs)
        {
            ValidateMaterialInputDTOList(inputDTOs);
            await CheckMaterialCodeDuplicate(inputDTOs);
            List<CreateUpdateResponseDTO<Material>> responses = new List<CreateUpdateResponseDTO<Material>>();
            foreach (MaterialInputDTO materialInputDTO in inputDTOs)
            {
                Material material = _mapper.Map<Material>(materialInputDTO);
                _materialRepository.Add(material);
                responses.Add(new CreateUpdateResponseDTO<Material>
                {
                    Code = material.Code,
                    Id = material.Id
                });
            }
            return responses;
        }

        public Task<MaterialDTO> Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<MaterialListingDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CreateUpdateResponseDTO<Material>> Update(MaterialInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<MaterialInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }
        private void ValidateMaterialInputDTOList(List<MaterialInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (MaterialInputDTO inputDTO in inputDTOs)
            {
                FluentValidation.Results.ValidationResult validationResult = _materialValidator.Validate(inputDTO);
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
                throw new APIException((int)HttpStatusCode.BadRequest, "Material list invalid", errors);
            }
        }
        private async Task CheckMaterialCodeDuplicate(List<MaterialInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (MaterialInputDTO materialInputDTO in inputDTOs)
            {
                Material codeDuplicatedMaterial =
                await _materialRepository.Search(material => material.Code.Equals(materialInputDTO.Code))
                                                    .FirstOrDefaultAsync();
                if (codeDuplicatedMaterial != null)
                {
                    errors.Add(new FormError
                    {
                        Property = typeof(MaterialInputDTO).GetProperty("Code").Name,
                        ErrorMessage = $"Material with code : {materialInputDTO.Code} duplicated"
                    });
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Code duplicate in material list", errors);
            }
        }
    }
}