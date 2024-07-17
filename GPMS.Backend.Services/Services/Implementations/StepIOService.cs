using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class StepIOService : IStepIOService
    {
        private readonly IGenericRepository<ProductionProcessStepIO> _stepIORepository;
        private readonly IValidator<StepIOInputDTO> _stepIOValidator;
        private readonly IMapper _mapper;

        public StepIOService(
            IGenericRepository<ProductionProcessStepIO> stepIORepository,
            IValidator<StepIOInputDTO> stepIOValidator,
            IMapper mapper)
        {
            _stepIORepository = stepIORepository;
            _stepIOValidator = stepIOValidator;
            _mapper = mapper;
        }

        public async Task AddList(List<StepIOInputDTO> inputDTOs, Guid stepId,
        List<CreateUpdateResponseDTO<Material>> materialCodeList,
        List<CreateUpdateResponseDTO<SemiFinishedProduct>> semiFinsihedProductCodeList)
        {
            ValidateStepIOInputDTOList(inputDTOs);
            ValidateMaterialCodeInStepIOWithMaterialCodeList(inputDTOs, materialCodeList);
            ValidateSemiFinishedProductCodeInStepIOWithSemiFinishedProductCodeList(inputDTOs, semiFinsihedProductCodeList);
            foreach (StepIOInputDTO stepIOInputDTO in inputDTOs)
            {
                ProductionProcessStepIO productionProcessStepIO = _mapper.Map<ProductionProcessStepIO>(stepIOInputDTO);
                productionProcessStepIO.ProductionProcessStepId = stepId;
                productionProcessStepIO.MaterialId = materialCodeList
                .First(materialCode => materialCode.Code.Equals(stepIOInputDTO.MaterialCode))
                .Id;
                productionProcessStepIO.SemiFinishedProductId = semiFinsihedProductCodeList
                .First(semiFinishedProductCode => semiFinishedProductCode.Code.Equals(stepIOInputDTO.SemiFinishedProductCode))
                .Id;
                _stepIORepository.Add(productionProcessStepIO);
            }
        }
        private void ValidateStepIOInputDTOList(List<StepIOInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (StepIOInputDTO inputDTO in inputDTOs)
            {
                FluentValidation.Results.ValidationResult validationResult = _stepIOValidator.Validate(inputDTO);
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
                throw new APIException((int)HttpStatusCode.BadRequest, "Step input output list invalid", errors);
            }
        }
        private void ValidateMaterialCodeInStepIOWithMaterialCodeList(List<StepIOInputDTO> inputDTOs,
        List<CreateUpdateResponseDTO<Material>> materialCodeList)
        {
            List<FormError> errors = new List<FormError>();
            foreach (StepIOInputDTO stepIOInputDTO in inputDTOs)
            {
                if (materialCodeList.FirstOrDefault(materialCode => materialCode.Code.Equals(stepIOInputDTO.MaterialCode)) == null)
                {
                    errors.Add(new FormError
                    {
                        Property = typeof(StepIOInputDTO).GetProperty("MaterialCode").Name,
                        ErrorMessage = $"Material code: {stepIOInputDTO.MaterialCode} of step input output :  is not valid"
                    });
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Material code in step input output list invalid", errors);
            }
        }
        private void ValidateSemiFinishedProductCodeInStepIOWithSemiFinishedProductCodeList(
            List<StepIOInputDTO> inputDTOs,
        List<CreateUpdateResponseDTO<SemiFinishedProduct>> semiFinishedProductCodeList)
        {
            List<FormError> errors = new List<FormError>();
            foreach (StepIOInputDTO stepIOInputDTO in inputDTOs)
            {
                if (semiFinishedProductCodeList.FirstOrDefault(
                    semiFinishedProductCode => semiFinishedProductCode.Code.Equals(stepIOInputDTO.SemiFinishedProductCode)) == null)
                {
                    errors.Add(new FormError
                    {
                        Property = typeof(StepIOInputDTO).GetProperty("SemiFinishedProductCode").Name,
                        ErrorMessage = $"Semi finished product code: {stepIOInputDTO.SemiFinishedProductCode} of step input output is not valid"
                    });
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Semi finished product code in step input output list invalid", errors);
            }
        }
    }
}