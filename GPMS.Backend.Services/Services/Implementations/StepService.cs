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
using GPMS.Backend.Services.Utils;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class StepService : IStepService
    {
        private readonly IGenericRepository<ProductionProcessStep> _stepRepository;
        private readonly IStepIOService _stepIOService;
        private readonly IValidator<StepInputDTO> _stepValidator;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;

        public StepService(
            IGenericRepository<ProductionProcessStep> stepRepository,
            IStepIOService stepIOService,
            IValidator<StepInputDTO> stepValidator,
            IMapper mapper,
            EntityListErrorWrapper entityListErrorWrapper)
        {
            _stepRepository = stepRepository;
            _stepIOService = stepIOService;
            _stepValidator = stepValidator;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
        }
        public async Task AddList(List<StepInputDTO> inputDTOs, Guid processId,
        List<CreateUpdateResponseDTO<Material>> materialCodeList,
        List<CreateUpdateResponseDTO<SemiFinishedProduct>> semiFinsihedProductCodeList)
        {
            ServiceUtils.ValidateInputDTOList<StepInputDTO, ProductionProcessStep>
                (inputDTOs, _stepValidator,_entityListErrorWrapper);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<StepInputDTO,ProductionProcessStep>
                (inputDTOs,"Code",_entityListErrorWrapper);
            await ServiceUtils.CheckFieldDuplicatedWithInputDTOListAndDatabase<StepInputDTO, ProductionProcessStep>
                (inputDTOs, _stepRepository, "Code", "Code",_entityListErrorWrapper);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<StepInputDTO,ProductionProcessStep>
                (inputDTOs,"OrderNumber",_entityListErrorWrapper);
            inputDTOs = inputDTOs.OrderBy(stepInputDTO => stepInputDTO.OrderNumber).ToList();
            foreach (StepInputDTO stepInputDTO in inputDTOs)
            {
                ProductionProcessStep productionProcessStep = _mapper.Map<ProductionProcessStep>(stepInputDTO);
                productionProcessStep.ProductionProcessId = processId;
                _stepRepository.Add(productionProcessStep);
                await _stepIOService.AddList(stepInputDTO.StepIOs, productionProcessStep.Id,
                materialCodeList, semiFinsihedProductCodeList);
            }
        }

        
    }
}