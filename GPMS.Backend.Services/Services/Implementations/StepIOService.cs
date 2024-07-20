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
            ServiceUtils.ValidateInputDTOList<StepIOInputDTO, ProductionProcessStepIO>(inputDTOs, _stepIOValidator);
            ServiceUtils.CheckForeignEntityCodeInInputDTOListExistedInForeignEntityCodeList<StepIOInputDTO, ProductionProcessStepIO, Material>
            (inputDTOs, materialCodeList, "MaterialCode");
            ServiceUtils.CheckForeignEntityCodeInInputDTOListExistedInForeignEntityCodeList<StepIOInputDTO, ProductionProcessStepIO, SemiFinishedProduct>
            (inputDTOs, semiFinsihedProductCodeList, "SemiFinishedProductCode");
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
    }
}