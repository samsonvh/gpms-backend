using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Enums.Types;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class StepIOService : IStepIOService
    {
        private readonly IGenericRepository<ProductionProcessStepIO> _stepIORepository;
        private readonly IGenericRepository<SemiFinishedProduct> _semiFinishedProduct;
        private readonly IValidator<StepIOInputDTO> _stepIOValidator;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly StepIOInputDTOWrapper _stepIOInputDTOWrapper;

        public StepIOService(
            IGenericRepository<ProductionProcessStepIO> stepIORepository,
            IGenericRepository<SemiFinishedProduct> semiFinishedProduct,
            IValidator<StepIOInputDTO> stepIOValidator,
            IMapper mapper,
            EntityListErrorWrapper entityListErrorWrapper,
            StepIOInputDTOWrapper stepIOInputDTOWrapper)
        {
            _stepIORepository = stepIORepository;
            _semiFinishedProduct = semiFinishedProduct;
            _stepIOValidator = stepIOValidator;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
            _stepIOInputDTOWrapper = stepIOInputDTOWrapper;
        }
        #region Add List
        public async Task AddList(List<StepIOInputDTO> inputDTOs, Guid stepId, 
        List<Guid> materialIds, 
        List<CreateUpdateResponseDTO<SemiFinishedProduct>> semiFinishedProductCodes)
        {
            ServiceUtils.ValidateInputDTOList<StepIOInputDTO, ProductionProcessStepIO>
                (inputDTOs, _stepIOValidator, _entityListErrorWrapper);
            var stepIOWithMaterialId = inputDTOs.Where(inputDTO => !inputDTO.MaterialId.IsNullOrEmpty()).ToList();
            var stepIOWithSemiFinishProductCode = inputDTOs.Where(inputDTO => !inputDTO.SemiFinishedProductCode.IsNullOrEmpty()).ToList();
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<StepIOInputDTO, ProductionProcessStepIO>
                (stepIOWithMaterialId,"MaterialId", _entityListErrorWrapper);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<StepIOInputDTO, ProductionProcessStepIO>
            (stepIOWithSemiFinishProductCode,"SemiFinishedProductCode", _entityListErrorWrapper);
            CheckContainsOnlyOneOutputAndAtLeastOneInput(inputDTOs);
            foreach (StepIOInputDTO stepIOInputDTO in inputDTOs)
            {
                ProductionProcessStepIO productionProcessStepIO = _mapper.Map<ProductionProcessStepIO>(stepIOInputDTO);
                productionProcessStepIO.ProductionProcessStepId = stepId;
                if (!stepIOInputDTO.MaterialId.IsNullOrEmpty())
                {
                    var existedMaterialId =
                    materialIds.FirstOrDefault(materialIds => materialIds.Equals(stepIOInputDTO.MaterialId));
                    if (existedMaterialId != null) productionProcessStepIO.MaterialId = existedMaterialId;
                }
                else if (!stepIOInputDTO.SemiFinishedProductCode.IsNullOrEmpty())
                {
                    var existedSemiFinishedProductCode =
                    semiFinishedProductCodes.FirstOrDefault(semiFinishedProductCode => semiFinishedProductCode.Code.Equals(stepIOInputDTO.SemiFinishedProductCode));
                    if (existedSemiFinishedProductCode != null) 
                    {
                        productionProcessStepIO.SemiFinishedProductId = existedSemiFinishedProductCode.Id;
                    }
                        
                }
                _stepIORepository.Add(productionProcessStepIO);
                _stepIOInputDTOWrapper.StepIOInputDTOList.Add(stepIOInputDTO);
            }
        }

        
        #endregion
        #region Get All StepIO
        public async Task<DefaultPageResponseListingDTO<StepIOListingDTO>> GetAll(StepIOFilterModel stepIOFilterModel)
        {
            var query = _stepIORepository.GetAll();
            query = Filters(query, stepIOFilterModel);
            query = query.SortBy<ProductionProcessStepIO>(stepIOFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery<ProductionProcessStepIO>(stepIOFilterModel);
            var stepIOs = await query.ProjectTo<StepIOListingDTO>(_mapper.ConfigurationProvider)
                                        .ToListAsync();
            return new DefaultPageResponseListingDTO<StepIOListingDTO>
            {
                Data = stepIOs,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = stepIOFilterModel.Pagination.PageIndex,
                    PageSize = stepIOFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }
        #endregion

        private IQueryable<ProductionProcessStepIO> Filters(IQueryable<ProductionProcessStepIO> query, StepIOFilterModel stepIOFilterModel)
        {
            if (Enum.TryParse(stepIOFilterModel.Type, true, out ProductionProcessStepIOType stepIOType))
            {
                query = query.Where(stepIO => stepIO.Type.Equals(stepIOFilterModel.Type));
            }

            if (stepIOFilterModel.IsProduct.HasValue)
            {
                query = query.Where(stepIO => stepIO.IsProduct == stepIOFilterModel.IsProduct.Value);
            }

            return query;
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