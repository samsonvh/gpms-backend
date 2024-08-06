using AutoMapper;
using AutoMapper.QueryableExtensions;
using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Results;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class StepResultService : IStepResultService
    {
        private readonly IGenericRepository<ProductionProcessStepResult> _stepResultRepository;
        private readonly IMapper _mapper;

        public StepResultService(IGenericRepository<ProductionProcessStepResult> stepResultRepository, IMapper mapper)
        {
            _stepResultRepository = stepResultRepository;
            _mapper = mapper;
        }

        public async Task<DefaultPageResponseListingDTO<StepResultListingDTO>> GetALlStepResultBySeries(Guid seriesId, StepResultFilterModel stepResultFilterModel)
        {
            IQueryable<ProductionProcessStepResult> query = _stepResultRepository
                .GetAll().Where(stepResult => stepResult.ProductionSeriesId == seriesId);
            query = Filter(query, stepResultFilterModel);
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

        private IQueryable<ProductionProcessStepResult> Filter(IQueryable<ProductionProcessStepResult> query, StepResultFilterModel stepResultFilterModel)
        {
            if (!stepResultFilterModel.Status.IsNullOrEmpty()
                    && Enum.TryParse<ProductionProcessStepResultStatus>(stepResultFilterModel.Status, true, out ProductionProcessStepResultStatus parsedProductionStepStatus))
            {
                query = query
                .Where(stepResult => stepResult.Status.Equals(parsedProductionStepStatus));
            }

            return query;
        }
    }
}
