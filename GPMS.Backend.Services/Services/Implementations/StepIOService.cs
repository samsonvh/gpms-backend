using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Enums.Types;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Utils;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class StepIOService : IStepIOService
    {
        private readonly IGenericRepository<ProductionProcessStepIO> _stepIORepository;
        private readonly IValidator<StepIOInputDTO> _stepIOValidator;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly StepIOInputDTOWrapper _stepIOInputDTOWrapper;

        public StepIOService(
            IGenericRepository<ProductionProcessStepIO> stepIORepository,
            IValidator<StepIOInputDTO> stepIOValidator,
            IMapper mapper,
            EntityListErrorWrapper entityListErrorWrapper,
            StepIOInputDTOWrapper stepIOInputDTOWrapper)
        {
            _stepIORepository = stepIORepository;
            _stepIOValidator = stepIOValidator;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
            _stepIOInputDTOWrapper = stepIOInputDTOWrapper;
        }

        public async Task AddList(List<StepIOInputDTO> inputDTOs, Guid stepId,
        List<CreateUpdateResponseDTO<Material>> materialCodeList,
        List<CreateUpdateResponseDTO<SemiFinishedProduct>> semiFinsihedProductCodeList)
        {
            ServiceUtils.ValidateInputDTOList<StepIOInputDTO, ProductionProcessStepIO>
                (inputDTOs, _stepIOValidator, _entityListErrorWrapper);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<StepIOInputDTO, ProductionProcessStepIO>
                (inputDTOs.Where(inputDTO => !inputDTO.MaterialCode.IsNullOrEmpty()).ToList(),
                    "MaterialCode", _entityListErrorWrapper);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<StepIOInputDTO, ProductionProcessStepIO>
                (inputDTOs.Where(inputDTO => !inputDTO.SemiFinishedProductCode.IsNullOrEmpty()).ToList(),
                    "SemiFinishedProductCode", _entityListErrorWrapper);
            ServiceUtils.CheckForeignEntityCodeInInputDTOListExistedInForeignEntityCodeList<StepIOInputDTO, ProductionProcessStepIO, Material>
                (inputDTOs.Where(inputDTO => !inputDTO.MaterialCode.IsNullOrEmpty()).ToList(),
                    materialCodeList, "MaterialCode", _entityListErrorWrapper);
            ServiceUtils.CheckForeignEntityCodeInInputDTOListExistedInForeignEntityCodeList<StepIOInputDTO, ProductionProcessStepIO, SemiFinishedProduct>
                (inputDTOs.Where(inputDTO => !inputDTO.SemiFinishedProductCode.IsNullOrEmpty()).ToList(),
                    semiFinsihedProductCodeList, "SemiFinishedProductCode", _entityListErrorWrapper);
            CheckContainsOnlyOneOutputAndAtLeastOneInput(inputDTOs);
            foreach (StepIOInputDTO stepIOInputDTO in inputDTOs)
            {
                ProductionProcessStepIO productionProcessStepIO = _mapper.Map<ProductionProcessStepIO>(stepIOInputDTO);
                productionProcessStepIO.ProductionProcessStepId = stepId;
                if (!stepIOInputDTO.MaterialCode.IsNullOrEmpty())
                {
                    CreateUpdateResponseDTO<Material> existedMaterialCode =
                    materialCodeList.FirstOrDefault(materialCode => materialCode.Code.Equals(stepIOInputDTO.MaterialCode));
                    if (existedMaterialCode != null) productionProcessStepIO.MaterialId = existedMaterialCode.Id;
                }
                else if (!stepIOInputDTO.SemiFinishedProductCode.IsNullOrEmpty())
                {
                    CreateUpdateResponseDTO<SemiFinishedProduct> existedSemiFinishedProductCode =
                    semiFinsihedProductCodeList.FirstOrDefault(semiFinishedProductCode => semiFinishedProductCode.Code.Equals(stepIOInputDTO.SemiFinishedProductCode));
                    if (existedSemiFinishedProductCode != null) productionProcessStepIO.SemiFinishedProductId = existedSemiFinishedProductCode.Id;
                }
                _stepIORepository.Add(productionProcessStepIO);
                _stepIOInputDTOWrapper.StepIOInputDTOList.Add(stepIOInputDTO);
            }
        }

        private void CheckContainsOnlyOneOutputAndAtLeastOneInput(List<StepIOInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            if (inputDTOs.Where(inputDTO => inputDTO.Type == ProductionProcessStepIOType.Output).Count() != 1)
            {
                errors.Add(new FormError
                {
                    Property = "Type",
                    ErrorMessage = "Step contain must contains 1 output only"
                });
            }
            if (inputDTOs.Where(inputDTO => inputDTO.Type == ProductionProcessStepIOType.Input).Count() < 1)
            {
                errors.Add(new FormError
                {
                    Property = "Type",
                    ErrorMessage = "Step contain at least 1 input"
                });
            }
            if (errors.Count > 0)
            {
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<ProductionProcessStepIO>(errors,_entityListErrorWrapper);
            }
        }
    }
}