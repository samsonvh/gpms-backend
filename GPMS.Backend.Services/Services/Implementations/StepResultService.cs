using System.Net;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Results;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Results;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class StepResultService : IStepResultService
    {
        private readonly IGenericRepository<ProductionProcessStepResult> _stepResultRepository;
        private readonly IGenericRepository<ProductionSeries> _seriesRepository;
        private readonly IGenericRepository<ProductionProcessStep> _stepRepository;
        private readonly IGenericRepository<ProductProductionProcess> _processRepository;
        private readonly IIOStepResultService _IOStepResultService;
        private readonly IValidator<StepResultInputDTO> _stepResultValidator;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly CurrentLoginUserDTO _currentLoginUserDTO;
        private readonly IMapper _mapper;

        public StepResultService
            (IGenericRepository<ProductionProcessStepResult> stepResultRepository,
            IGenericRepository<ProductionSeries> seriesRepository,
            IGenericRepository<ProductionProcessStep> stepRepository,
            IGenericRepository<ProductProductionProcess> processRepository,
            IIOStepResultService IOStepResultService,
            IValidator<StepResultInputDTO> stepResultValidator,
            CurrentLoginUserDTO currentLoginUserDTO,
            EntityListErrorWrapper entityListErrorWrapper,
                IMapper mapper)
        {
            _stepResultRepository = stepResultRepository;
            _seriesRepository = seriesRepository;
            _stepRepository = stepRepository;
            _processRepository = processRepository;
            _IOStepResultService = IOStepResultService;
            _stepResultValidator = stepResultValidator;
            _entityListErrorWrapper = entityListErrorWrapper;
            _currentLoginUserDTO = currentLoginUserDTO;
            _mapper = mapper;
        }
        #region Create Step Result By Series Id
        public async Task<StepResultDTO> Add(Guid seriesId, StepResultInputDTO inputDTO)
        {
            ServiceUtils.ValidateInputDTO<StepResultInputDTO, ProductionProcessStepResult>
                (inputDTO, _stepResultValidator, _entityListErrorWrapper);
            CheckUserRole();
            var existedSeries = await _seriesRepository.Search(series => series.Id.Equals(seriesId))
                                                .Include(series => series.ProductionEstimation)
                                                    .ThenInclude(estimation => estimation.ProductionRequirement)
                                                        .ThenInclude(requirement => requirement.ProductSpecification)
                                                .FirstOrDefaultAsync();
            ValidateExistedSeries(existedSeries);
            var seriesProductId = existedSeries.ProductionEstimation.ProductionRequirement.ProductSpecification.ProductId;
            var existedStep = await _stepRepository.Search(step => step.Id.Equals(inputDTO.StepId))
                                                    .Include(step => step.ProductionProcess)
                                                    .FirstOrDefaultAsync();
            var stepProductId = existedStep.ProductionProcess.ProductId;
            if (!seriesProductId.Equals(stepProductId))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Cannot enter Step Result because Step and Series must be in the same Product");
            }
            if (!existedStep.ProductionProcess.Name.Equals(existedSeries.CurrentProcess))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Cannot enter Step Result because Step is not in Current Process of Series");
            }
            var stepResult = HandleAddStepResult(inputDTO, seriesId);
            await _IOStepResultService.AddList(inputDTO.StepId, stepResult.Id, inputDTO.inputOutputResults);
            if (_entityListErrorWrapper.EntityListErrors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Create Step Result Failed", _entityListErrorWrapper);
            }
            var stepWithNoResultCount =  _stepRepository.Search(step => step.ProductionProcessStepIOs.Any(s => s.ProductionProcessStepIOResults.Count == 0)).Count();
            if (stepWithNoResultCount == 0)
            {
                int nextProcessOrder = existedStep.ProductionProcess.OrderNumber + 1;
                var inspectionProcess = await _processRepository.Search(process => process.Name.Equals("Inspection")).FirstOrDefaultAsync();
                var nextProcess =  await _processRepository.Search(
                    process => process.OrderNumber.Equals(nextProcessOrder) && process.ProductId.Equals(stepProductId)
                    && process.OrderNumber < inspectionProcess.OrderNumber
                    ).FirstOrDefaultAsync();
                existedSeries.CurrentProcess = nextProcess.Name;
                _seriesRepository.Update(existedSeries);
            }
            _stepResultRepository.Add(stepResult);
            await _stepResultRepository.Save();
            return _mapper.Map<StepResultDTO>(stepResult);
        }

        private ProductionProcessStepResult HandleAddStepResult(StepResultInputDTO inputDTO, Guid seriesId)
        {
            var stepResult = _mapper.Map<ProductionProcessStepResult>(inputDTO);
            stepResult.CreatorId = _currentLoginUserDTO.Id;
            stepResult.ProductionSeriesId = seriesId;
            stepResult.ProductionProcessStepId = inputDTO.StepId;
            return stepResult;
        }

        private void CheckUserRole()
        {
            if (!_currentLoginUserDTO.Department.Equals("Production Department"))
            {
                throw new APIException((int)HttpStatusCode.Unauthorized, "Only Production Staff are allowed to enter Step Results.");
            }
        }

        private void ValidateExistedSeries(ProductionSeries? existedSeries)
        {
            if (existedSeries == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Series Not Found");
            }
            if (!existedSeries.Status.Equals(ProductionSeriesStatus.InProduction))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Can not enter Step Result to Series which Status is not InProduction");
            }
        }
        #endregion

        public async Task<DefaultPageResponseListingDTO<StepResultListingDTO>> GetALlStepResultBySeries(Guid seriesId, StepResultFilterModel stepResultFilterModel)
        {
            IQueryable<ProductionProcessStepResult> query = _stepResultRepository
                .GetAll().Where(stepResult => stepResult.ProductionSeriesId == seriesId);
            int totalItem = query.Count();
            query = query.SortBy<ProductionProcessStepResult>(stepResultFilterModel);
            query = query.PagingEntityQuery(stepResultFilterModel);
            var data = await query.ProjectTo<StepResultListingDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return new DefaultPageResponseListingDTO<StepResultListingDTO>
            {
                Data = data,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = stepResultFilterModel.Pagination.PageIndex,
                    PageSize = stepResultFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }

    }
}
