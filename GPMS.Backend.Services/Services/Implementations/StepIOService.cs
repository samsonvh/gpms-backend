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
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<StepIOInputDTO, ProductionProcessStepIO>
            (inputDTOs.Where(inputDTO => !inputDTO.MaterialCode.IsNullOrEmpty()).ToList(), "MaterialCode");
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<StepIOInputDTO, ProductionProcessStepIO>
            (inputDTOs.Where(inputDTO => !inputDTO.SemiFinishedProductCode.IsNullOrEmpty()).ToList(), "SemiFinishedProductCode");
            ServiceUtils.CheckForeignEntityCodeInInputDTOListExistedInForeignEntityCodeList<StepIOInputDTO, ProductionProcessStepIO, Material>
            (inputDTOs.Where(inputDTO => !inputDTO.MaterialCode.IsNullOrEmpty()).ToList(), materialCodeList, "MaterialCode");
            ServiceUtils.CheckForeignEntityCodeInInputDTOListExistedInForeignEntityCodeList<StepIOInputDTO, ProductionProcessStepIO, SemiFinishedProduct>
            (inputDTOs.Where(inputDTO => !inputDTO.SemiFinishedProductCode.IsNullOrEmpty()).ToList(), semiFinsihedProductCodeList, "SemiFinishedProductCode");
            CheckContainsOnlyOneOutputAndAtLeastOneInput(inputDTOs);
            foreach (StepIOInputDTO stepIOInputDTO in inputDTOs)
            {
                ProductionProcessStepIO productionProcessStepIO = _mapper.Map<ProductionProcessStepIO>(stepIOInputDTO);
                productionProcessStepIO.ProductionProcessStepId = stepId;
                if (!stepIOInputDTO.MaterialCode.IsNullOrEmpty())
                {
                    productionProcessStepIO.MaterialId = materialCodeList
                    .First(materialCode => materialCode.Code.Equals(stepIOInputDTO.MaterialCode))
                    .Id;
                }
                else if (!stepIOInputDTO.SemiFinishedProductCode.IsNullOrEmpty())
                {
                    productionProcessStepIO.SemiFinishedProductId = semiFinsihedProductCodeList
                    .First(semiFinishedProductCode => semiFinishedProductCode.Code.Equals(stepIOInputDTO.SemiFinishedProductCode))
                    .Id;
                }
                _stepIORepository.Add(productionProcessStepIO);
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
                throw new APIException((int)HttpStatusCode.BadRequest, "Invalid Type in Step Input Output", errors);
            }
        }
    }
}