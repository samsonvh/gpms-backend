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
    public class StepService : IStepService
    {
        private readonly IGenericRepository<ProductionProcessStep> _stepRepository;
        private readonly IStepIOService _stepIOService;
        private readonly IValidator<StepInputDTO> _stepValidator;
        private readonly IMapper _mapper;

        public StepService(
            IGenericRepository<ProductionProcessStep> stepRepository,
            IStepIOService stepIOService,
            IValidator<StepInputDTO> stepValidator,
            IMapper mapper)
        {
            _stepRepository = stepRepository;
            _stepIOService = stepIOService;
            _stepValidator = stepValidator;
            _mapper = mapper;
        }
        public async Task AddList(List<StepInputDTO> inputDTOs, Guid processId,
        List<CreateUpdateResponseDTO<Material>> materialCodeList,
        List<CreateUpdateResponseDTO<SemiFinishedProduct>> semiFinsihedProductCodeList)
        {
            ValidateStepInputDTOList(inputDTOs);
            inputDTOs = inputDTOs.OrderBy(stepInputDTO => stepInputDTO.OrderNumber).ToList();
            foreach (StepInputDTO stepInputDTO in inputDTOs)
            {
                ProductionProcessStep productionProcessStep = _mapper.Map<ProductionProcessStep>(stepInputDTO);
                productionProcessStep.ProductionProcessId = processId;
                _stepRepository.Add(productionProcessStep);
                await _stepIOService.AddList(stepInputDTO.StepIOs, productionProcessStep.Id,
                materialCodeList,semiFinsihedProductCodeList);
            }
        }
        private void ValidateStepInputDTOList(List<StepInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (StepInputDTO inputDTO in inputDTOs)
            {
                FluentValidation.Results.ValidationResult validationResult = _stepValidator.Validate(inputDTO);
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
                throw new APIException((int)HttpStatusCode.BadRequest, "Step list invalid", errors);
            }
        }
    }
}