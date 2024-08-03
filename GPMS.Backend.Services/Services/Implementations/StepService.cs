using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using FluentValidation.Results;
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

        #region Get All Step
        public async Task<DefaultPageResponseListingDTO<StepListingDTO>> GetAll(StepFilterModel stepFilterModel)
        {
            var query = _stepRepository.GetAll();
            query = Filters(query, stepFilterModel);
            query = query.SortBy<ProductionProcessStep>(stepFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery<ProductionProcessStep>(stepFilterModel);
            var steps = await query.ProjectTo<StepListingDTO>(_mapper.ConfigurationProvider)
                                        .ToListAsync();
            return new DefaultPageResponseListingDTO<StepListingDTO>
            {
                Data = steps,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = stepFilterModel.Pagination.PageIndex,
                    PageSize = stepFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }
        #endregion

        private IQueryable<ProductionProcessStep> Filters(IQueryable<ProductionProcessStep> query, StepFilterModel stepFilterModel)
        {
            if (!stepFilterModel.Code.IsNullOrEmpty())
            {
                query = query.Where(measurement => measurement.Code.Contains(stepFilterModel.Code));
            }

            if (!stepFilterModel.Name.IsNullOrEmpty())
            {
                query = query.Where(measurement => measurement.Name.Contains(stepFilterModel.Name));
            }
            return query;
        }
    }
}