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
    public class BillOfMaterialService : IBillOfMaterialService
    {
        private readonly IGenericRepository<BillOfMaterial> _billOfMaterialRepository;
        private readonly IValidator<BOMInputDTO> _billOfMaterialValidator;
        private readonly IMapper _mapper;
        public BillOfMaterialService(
            IGenericRepository<BillOfMaterial> billOfMaterialRepository,
            IValidator<BOMInputDTO> billOfMaterialValidator,
            IMapper mapper
            )
        {
            _billOfMaterialRepository = billOfMaterialRepository;
            _billOfMaterialValidator = billOfMaterialValidator;
            _mapper = mapper;
        }

        public Task<CreateUpdateResponseDTO<BillOfMaterial>> Add(BOMInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<BOMInputDTO> inputDTOs, Guid? parentEntityId = null)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<BOMInputDTO> inputDTOs, Guid specificationId, 
        List<CreateUpdateResponseDTO<Material>> materialCodeList)
        {
            ValidateBOMInputDTOList(inputDTOs);
            ValidateMaterialCodeInBOMWithMaterialCodeList(inputDTOs, materialCodeList);
            foreach (BOMInputDTO bomInputDTO in inputDTOs)
            {
                BillOfMaterial billOfMaterial = _mapper.Map<BillOfMaterial>(bomInputDTO);
                billOfMaterial.ProductSpecificationId = specificationId;
                billOfMaterial.MaterialId = materialCodeList
                .First(materialCode => materialCode.Code.Equals(bomInputDTO.MaterialCode))
                .Id;
                _billOfMaterialRepository.Add(billOfMaterial);
            }
        }

        public Task<BOMDTO> Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BOMListingDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CreateUpdateResponseDTO<BillOfMaterial>> Update(BOMInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<BOMInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }
        private void ValidateBOMInputDTOList(List<BOMInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (BOMInputDTO inputDTO in inputDTOs)
            {
                FluentValidation.Results.ValidationResult validationResult = _billOfMaterialValidator.Validate(inputDTO);
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
                throw new APIException((int)HttpStatusCode.BadRequest, "Bill of material list invalid", errors);
            }
        }
        private void ValidateMaterialCodeInBOMWithMaterialCodeList(List<BOMInputDTO> inputDTOs,
        List<CreateUpdateResponseDTO<Material>> materialCodeList)
        {
            List<FormError> errors = new List<FormError>();
            foreach (BOMInputDTO bOmInputDTO in inputDTOs)
            {
                if (materialCodeList.FirstOrDefault(materialCode => materialCode.Code.Equals(bOmInputDTO.MaterialCode)) == null)
                {
                    errors.Add(new FormError
                    {
                        Property = typeof(BOMInputDTO).GetProperty("MaterialCode").Name,
                        ErrorMessage = $"Material code: {bOmInputDTO.MaterialCode} of BOM is not valid"
                    });
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Material code in bill of material list invalid", errors);
            }
        }
    }
}