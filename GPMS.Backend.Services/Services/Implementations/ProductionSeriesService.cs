using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Data.Enums.Times;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.PageRequests;
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
    public class ProductionSeriesService : IProductionSeriesService
    {
        private readonly IGenericRepository<ProductionSeries> _productionSeriesRepository;
        private readonly IGenericRepository<ProductionEstimation> _productionEstimationRepository;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly IValidator<ProductionSeriesInputDTO> _productionSeriesValidator;

        public ProductionSeriesService(
            IGenericRepository<ProductionSeries> productionSeriesRepository,
            IGenericRepository<ProductionEstimation> productionEstimationRepository,
            IMapper mapper,
            EntityListErrorWrapper entityListErrorWrapper,
            IValidator<ProductionSeriesInputDTO> productionSeriesValidator)
        {
            _productionSeriesRepository = productionSeriesRepository;
            _productionEstimationRepository = productionEstimationRepository;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
            _productionSeriesValidator = productionSeriesValidator;
        }

        public async Task AddList(List<ProductionSeriesInputDTO> inputDTOs, Guid productionEstimationId)
        {
            ServiceUtils.ValidateInputDTOList<ProductionSeriesInputDTO, ProductionSeries>
                (inputDTOs, _productionSeriesValidator, _entityListErrorWrapper);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<ProductionSeriesInputDTO, ProductionSeries>
                (inputDTOs, "Code", _entityListErrorWrapper);
            await ServiceUtils.CheckFieldDuplicatedWithInputDTOListAndDatabase<ProductionSeriesInputDTO, ProductionSeries>
                (inputDTOs, _productionSeriesRepository, "Code", "Code", _entityListErrorWrapper);
            CheckSeriesQuantityWithProductionEstimation(inputDTOs, productionEstimationId);
            foreach (ProductionSeriesInputDTO productionSeriesInputDTO in inputDTOs)
            {
                ProductionSeries productionSeries = _mapper.Map<ProductionSeries>(productionSeriesInputDTO);
                productionSeries.ProductionEstimationId = productionEstimationId;
                productionSeries.Status = ProductionSeriesStatus.Pending;
                _productionSeriesRepository.Add(productionSeries);
            }
        }

        #region Get All Series Of Estimation
        public async Task<DefaultPageResponseListingDTO<ProductionSeriesListingDTO>> GetAllSeriesOfEstimation(Guid estimationId, ProductionSeriesFilterModel productionSeriesFilterModel)
        {
            IQueryable<ProductionSeries> query = _productionSeriesRepository
                .GetAll().Where(series => series.ProductionEstimationId == estimationId);
            query = Filter(query, productionSeriesFilterModel);
            int totalItem = query.Count();
            query = query.SortBy<ProductionSeries>(productionSeriesFilterModel);
            query = query.PagingEntityQuery(productionSeriesFilterModel);
            var data = await query.ProjectTo<ProductionSeriesListingDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return new DefaultPageResponseListingDTO<ProductionSeriesListingDTO>
            {
                Data = data,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = productionSeriesFilterModel.Pagination.PageIndex,
                    PageSize = productionSeriesFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }

        #endregion GetAll Sereies Of Estimation

        private IQueryable<ProductionSeries> Filter(IQueryable<ProductionSeries> query, ProductionSeriesFilterModel productionSeriesFilterModel)
        {
            if (!productionSeriesFilterModel.Code.IsNullOrEmpty())
            {
                query = query.Where(productionSeries => productionSeries.Code.ToLower()
                                .Contains(productionSeriesFilterModel.Code.ToLower()));
            }

            if (!productionSeriesFilterModel.Status.IsNullOrEmpty()
                    && Enum.TryParse<ProductionSeriesStatus>(productionSeriesFilterModel.Status, true, out ProductionSeriesStatus parsedProductionSeriesStatus))
            {
                query = query
                .Where(product => product.Status.Equals(parsedProductionSeriesStatus));
            }

            return query;
        }

        private async void CheckSeriesQuantityWithProductionEstimation(List<ProductionSeriesInputDTO> inputDTOs, Guid productionEstimationId)
        {
            List<FormError> errors = new List<FormError>();
            ProductionEstimation productionEstimation = _productionEstimationRepository.GetUnAddedEntityById(productionEstimationId);
            int totalSum = 0;
            foreach (ProductionSeriesInputDTO inputDTO in inputDTOs)
            {
                totalSum += inputDTO.Quantity;
            }
            if (totalSum != productionEstimation.Quantity)
            {
                errors.Add(new FormError
                {
                    ErrorMessage = "Total quantity of Production Series List must equal quantity of Production Estimation",
                    Property = "Quantity",
                    EntityOrder = 1
                });
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<ProductionSeries>(errors,_entityListErrorWrapper);
            }
        }

        #region Get All By Reqs Id And Day Number
        public async Task<DefaultPageResponseListingDTO<ProductionSeriesListingDTO>> GetAllSeriesByRequirementIdAndDayNumber
            (Guid requirementId,ProductionSeriesFilterModel productionSeriesFilterModel)
        {
            var query = _productionSeriesRepository
            .Search(series => series.ProductionEstimation.DayNumber.Equals(productionSeriesFilterModel.DayNumber)
                            && series.ProductionEstimation.ProductionRequirementId.Equals(requirementId));
            query = query.SortBy(productionSeriesFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery(productionSeriesFilterModel);
            var data = await query.ProjectTo<ProductionSeriesListingDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return new DefaultPageResponseListingDTO<ProductionSeriesListingDTO>
            {
                Data = data,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = productionSeriesFilterModel.Pagination.PageIndex,
                    PageSize = productionSeriesFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }
        #endregion
    }
}
