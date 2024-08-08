using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Data.Enums.Types;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class ProductionRequirementService : IProductionRequirementService
    {
        private readonly IGenericRepository<ProductionRequirement> _productionRequirementRepository;
        private readonly IGenericRepository<ProductSpecification> _productSpecificationRepository;
        private readonly IGenericRepository<ProductionPlan> _productionPlanRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductionRequirementInputDTO> _productionRequirementValidator;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly IProductionEstimationService _productionEstimationService;

        public ProductionRequirementService(
            IGenericRepository<ProductionRequirement> productionRequirementRepository,
            IGenericRepository<ProductSpecification> productSpecificationRepository,
            IGenericRepository<ProductionPlan> productionPlanRepository,
            IMapper mapper,
            IValidator<ProductionRequirementInputDTO> productionRequirementValidator,
            EntityListErrorWrapper entityListErrorWrapper,
            IProductionEstimationService productionEstimationService)
        {
            _productionRequirementRepository = productionRequirementRepository;
            _productSpecificationRepository = productSpecificationRepository;
            _productionPlanRepository = productionPlanRepository;
            _mapper = mapper;
            _productionRequirementValidator = productionRequirementValidator;
            _entityListErrorWrapper = entityListErrorWrapper;
            _productionEstimationService = productionEstimationService;
        }

        public async Task AddRequirementListForAnnualProductionPlan(List<ProductionRequirementInputDTO> inputDTOs, Guid productionPlanId)
        {
            ServiceUtils.ValidateInputDTOList<ProductionRequirementInputDTO, ProductionRequirement>
                (inputDTOs, _productionRequirementValidator, _entityListErrorWrapper);
            foreach (ProductionRequirementInputDTO inputDTO in inputDTOs)
            {
                await ValidateSpecification(inputDTO.ProductionSpecificationId, inputDTOs.IndexOf(inputDTO) + 1);

                var productionRequirement = _mapper.Map<ProductionRequirement>(inputDTO);
                productionRequirement.ProductionPlanId = productionPlanId;
                productionRequirement.ProductSpecificationId = inputDTO.ProductionSpecificationId;

                _productionRequirementRepository.Add(productionRequirement);
                await _productionEstimationService.AddEstimationListForAnnualProductionPlan(inputDTO.ProductionEstimations, productionRequirement.Id, productionPlanId);

            }

        }

        public async Task AddRequirementListForChildProductionPlan(List<ProductionRequirementInputDTO> inputDTOs, Guid productionPlanId)
        {
            ServiceUtils.ValidateInputDTOList<ProductionRequirementInputDTO, ProductionRequirement>
                (inputDTOs, _productionRequirementValidator, _entityListErrorWrapper);
            foreach (ProductionRequirementInputDTO inputDTO in inputDTOs)
            {
                await ValidateSpecification(inputDTO.ProductionSpecificationId, inputDTOs.IndexOf(inputDTO) + 1);

                var productionRequirement = _mapper.Map<ProductionRequirement>(inputDTO);
                productionRequirement.ProductionPlanId = productionPlanId;
                productionRequirement.ProductSpecificationId = inputDTO.ProductionSpecificationId;

                _productionRequirementRepository.Add(productionRequirement);
                await _productionEstimationService
                    .AddEstimationListForChildProductionPlan
                    (inputDTO.ProductionEstimations, productionRequirement.Id, productionPlanId);
            }
        }

        public async Task<DefaultPageResponseListingDTO<ProductionRequirementListingDTO>> GetAllByProductionPlanId(Guid productionPlanId, RequirementFilterModel requirementFilterModel)
        {
            var query = _productionRequirementRepository
                            .Search(requirement => requirement.ProductionPlanId.Equals(productionPlanId));
            query = query.SortBy(requirementFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery(requirementFilterModel);
            var result = await query.ProjectTo<ProductionRequirementListingDTO>(_mapper.ConfigurationProvider).ToListAsync();
            DefaultPageResponseListingDTO<ProductionRequirementListingDTO> resposne =
            new DefaultPageResponseListingDTO<ProductionRequirementListingDTO>
            {
                Pagination = new PaginationResponseModel
                {
                    PageIndex = requirementFilterModel.Pagination.PageIndex,
                    PageSize = requirementFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                },
                Data = result
            };
            return resposne;
        }
        #region Get All  Available Series By Plan Id 
        public async Task<DefaultPageResponseListingDTO<ProductionRequirementStepResultListingDTO>>
            GetAllRequirementHaveAvailableSeriesAtCurrentDayByProductionPlanId
            (Guid productionPlanId, RequirementFilterModel requirementFilterModel)
        {
            var today = DateTime.Now;
            var existedProductionPlan = await _productionPlanRepository
                        .Search(plan => plan.Id.Equals(productionPlanId) && plan.Status.Equals(ProductionPlanStatus.InProgress))
                        .FirstOrDefaultAsync();
            if (existedProductionPlan == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Production Plan Not Found");
            }
            DateTime startDay = (DateTime)existedProductionPlan.ActualStartingDate;
            DateTime dueDay = (DateTime)existedProductionPlan.CompletionDate;
            if (today < startDay || today > dueDay)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Today is out of the range of actual start day and due date");
            }
            int dayNumber = (today - startDay).Days + 1;
            var query = _productionRequirementRepository
                            .Search(requirement => requirement.ProductionPlanId.Equals(productionPlanId)
                                    && requirement.ProductionEstimations
                                    .Where(estimation => estimation.DayNumber == dayNumber)
                                    .SelectMany(estimation => estimation.ProductionSeries)
                                    .Any(series => !series.Status.Equals(ProductionSeriesStatus.Pending)
                                            && !series.Equals(ProductionSeriesStatus.Done)));
            query = query.SortBy(requirementFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery(requirementFilterModel);
            var result = await query.ProjectTo<ProductionRequirementStepResultListingDTO>(_mapper.ConfigurationProvider).ToListAsync();
            foreach (var requirementListingDTO in result)
            {
                requirementListingDTO.DayNumber = dayNumber;
            }
            DefaultPageResponseListingDTO<ProductionRequirementStepResultListingDTO> resposne =
            new DefaultPageResponseListingDTO<ProductionRequirementStepResultListingDTO>
            {
                Pagination = new PaginationResponseModel
                {
                    PageIndex = requirementFilterModel.Pagination.PageIndex,
                    PageSize = requirementFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                },
                Data = result
            };
            return resposne;
        }
        #endregion
        private async Task ValidateSpecification(Guid productSpecificationId, int entityOrder)
        {
            List<FormError> errors = new List<FormError>();
            ProductSpecification productionSpecification = await _productSpecificationRepository
                .Search(spec => spec.Id == productSpecificationId)
                .Include(specification => specification.Product)
                .FirstOrDefaultAsync();

            if (productionSpecification == null)
            {
                errors.Add(new FormError
                {
                    EntityOrder = entityOrder,
                    ErrorMessage = $"Specification Id : {productSpecificationId} Not Found",
                    Property = "ProductionSpecificationId"
                });
            }
            else
            {
                if (!(productionSpecification.Product.Status.Equals(ProductStatus.Approved)
                    || productionSpecification.Product.Status.Equals(ProductStatus.InProduction)))
                {
                    errors.Add(new FormError
                    {
                        EntityOrder = entityOrder,
                        ErrorMessage = $"Product with Specification Id : {productSpecificationId} can not add into production plan",
                        Property = "ProductionSpecificationId"
                    });
                }
            }
            if (errors.Count > 0)
            {
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<ProductionRequirement>(errors, _entityListErrorWrapper);
            }
        }
    }
}
