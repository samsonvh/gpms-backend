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
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class ProcessService : IProcessService
    {
        private readonly IGenericRepository<ProductProductionProcess> _processRepository;
        private readonly IStepService _stepService;
        private readonly IValidator<ProcessInputDTO> _processValidator;
        private readonly IMapper _mapper;

        public ProcessService(
            IGenericRepository<ProductProductionProcess> processRepository,
            IStepService stepService,
            IValidator<ProcessInputDTO> processValidator,
            IMapper mapper
            )
        {
            _processRepository = processRepository;
            _stepService = stepService;
            _processValidator = processValidator;
            _mapper = mapper;
        }

        public Task<CreateUpdateResponseDTO<ProductProductionProcess>> Add(ProcessInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<ProcessInputDTO> inputDTOs, Guid? productId = null)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<ProcessInputDTO> inputDTOs, Guid productId, 
        List<CreateUpdateResponseDTO<Material>> materialCodeList, 
        List<CreateUpdateResponseDTO<SemiFinishedProduct>> semiFinishedProductCodeList)
        {
            ValidateProcessInputDTOList(inputDTOs);
            inputDTOs = inputDTOs.OrderBy(processInputDTO => processInputDTO.OrderNumber).ToList();
            foreach (ProcessInputDTO processInputDTO in inputDTOs)
            {
                ProductProductionProcess productProductionProcess = _mapper.Map<ProductProductionProcess>(processInputDTO);
                productProductionProcess.ProductId = productId;
                _processRepository.Add(productProductionProcess);
                await _stepService.AddList(processInputDTO.Steps, productProductionProcess.Id,
                materialCodeList,semiFinishedProductCodeList);
            }
        }

        public Task<ProcessDTO> Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ProcessListingDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CreateUpdateResponseDTO<ProductProductionProcess>> Update(ProcessInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<ProcessInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }
        private void ValidateProcessInputDTOList(List<ProcessInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (ProcessInputDTO inputDTO in inputDTOs)
            {
                ValidationResult validationResult = _processValidator.Validate(inputDTO);
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
                throw new APIException((int)HttpStatusCode.BadRequest, "Process list invalid", errors);
            }
        }
    }
}