using AutoMapper;
using FluentValidation;
using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Data.Enums.Types;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.PageRequests;
using GPMS.Backend.Services.Utils;
using GPMS.Backend.Services.Utils.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class ProductionPlanService : IProductionPlanService
    {
        private readonly IGenericRepository<ProductionPlan> _productionPlanRepository;
        private readonly IValidator<ProductionPlanInputDTO> _productionPlanValidator;
        private readonly IMapper _mapper;
        private readonly IProductionRequirementService _productionRequirementService;
        private readonly IProductionEstimationService _productionEstimationService;
        private readonly IProductionSeriesService _productionSeriesService;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;

        public ProductionPlanService(IGenericRepository<ProductionPlan> productionPlanRepository,
                                    IValidator<ProductionPlanInputDTO> productionPlanValidator,
                                    IMapper mapper,
                                    IProductionRequirementService productionRequirementService,
                                    IProductionEstimationService productionEstimationService,
                                    IProductionSeriesService productionSeriesService,
                                    EntityListErrorWrapper entityListErrorWrapper)
        {
            _productionPlanRepository = productionPlanRepository;
            _productionPlanValidator = productionPlanValidator;
            _mapper = mapper;
            _productionRequirementService = productionRequirementService;
            _productionEstimationService = productionEstimationService;
            _productionSeriesService = productionSeriesService;
            _entityListErrorWrapper = entityListErrorWrapper;
        }

        private async Task<ProductionPlan> HandleAddProductionPlan(ProductionPlanInputDTO inputDTO, CurrentLoginUserDTO currentLoginUserDTO)
        {
            ProductionPlan productionPlan = _mapper.Map<ProductionPlan>(inputDTO);
            productionPlan.CreatorId = currentLoginUserDTO.StaffId;
            productionPlan.Status = ProductionPlanStatus.Pending;
            _productionPlanRepository.Add(productionPlan);
            return productionPlan;
        }


        public async Task<CreateUpdateResponseDTO<ProductionPlan>> Add(ProductionPlanInputDTO inputDTO, CurrentLoginUserDTO currentLoginUserDTO)
        {

            ServiceUtils.ValidateInputDTO<ProductionPlanInputDTO, ProductionPlan>
            (inputDTO, _productionPlanValidator, _entityListErrorWrapper);

              await ServiceUtils.CheckFieldDuplicatedWithInputDTOAndDatabase<ProductionPlanInputDTO, ProductionPlan>(
                inputDTO, _productionPlanRepository, "Code", "Code", _entityListErrorWrapper);

            ProductionPlan productionPlan = await HandleAddProductionPlan(inputDTO, currentLoginUserDTO);

            await _productionPlanRepository.Save();

            foreach (var requirementDTO in inputDTO.ProductionRequirementInputDTOs)
            {
                var requirementResponses = await _productionRequirementService.AddList(new List<ProductionRequirementInputDTO> { requirementDTO }, productionPlan.Id);

                foreach (var requirementResponse in requirementResponses)
                {
                    foreach (var estimationDTO in requirementDTO.ProductionEstimationInputDTOs)
                    {
                        var estimationResponses = await _productionEstimationService.AddList(new List<ProductionEstimationInputDTO> { estimationDTO }, requirementResponse.Id);

                        foreach (var estimationResponse in estimationResponses)
                        {
                            await _productionSeriesService.AddList(estimationDTO.ProductionSeriesInputDTOs.ToList(), estimationResponse.Id);
                        }
                    }
                }
            }

            return new CreateUpdateResponseDTO<ProductionPlan>
            {
                Id = productionPlan.Id,
                Code = productionPlan.Code,
            };
        }

        public Task<CreateUpdateResponseDTO<ProductionPlan>> Add(ProductionPlanInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task AddList(List<ProductionPlanInputDTO> inputDTOs, Guid? parentEntityId = null)
        {
            throw new NotImplementedException();
        }

        public Task<CreateUpdateResponseDTO<ProductionPlan>> Update(ProductionPlanInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<ProductionPlanInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductionPlanListingDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductionPlanDTO> Details(Guid id)
        {
            var productionPlan = await _productionPlanRepository
                .Search(productionPlan => productionPlan.Id == id)
                .Include(productionPlan => productionPlan.ProductionRequirements)
                    .ThenInclude(productionRequirement => productionRequirement.ProductSpecification) 
                .Include(productionPlan => productionPlan.ProductionRequirements)
                    .ThenInclude(productionRequirement => productionRequirement.ProductionEstimations)
               .ThenInclude(productionEstimation => productionEstimation.ProductionSeries) 
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            if (productionPlan == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, $"Production Plan with ID: {id} not found");
            }
            return _mapper.Map<ProductionPlanDTO>(productionPlan);
        }

        public async Task<DefaultPageResponseListingDTO<ProductionPlanListingDTO>> GetAll(ProductionPlanPageRequest productionPlanPageRequest)
        {
            IQueryable<ProductionPlan> query = _productionPlanRepository.GetAll();
            query = Filter(query, productionPlanPageRequest);
            query = query.SortByAndPaging(productionPlanPageRequest);
            List<ProductionPlan> productionPlanList = await query.ToListAsync();
            int totalItem = productionPlanList.Count;
            productionPlanList = productionPlanList.PagingEntityList(productionPlanPageRequest);
            List<ProductionPlanListingDTO> productionPlanListingDTOs = new List<ProductionPlanListingDTO>();
            foreach(ProductionPlan productionPlan in productionPlanList)
            {
                ProductionPlanListingDTO productionPlanListingDTO = _mapper.Map<ProductionPlanListingDTO>(productionPlan);
                productionPlanListingDTOs.Add(productionPlanListingDTO);
            }

            int pageCount = totalItem / productionPlanPageRequest.PageSize;
            if (totalItem % productionPlanPageRequest.PageSize > 0)
            {
                pageCount += 1;
            }

            DefaultPageResponseListingDTO<ProductionPlanListingDTO> defaultPageResponseListingDTO =
                new DefaultPageResponseListingDTO<ProductionPlanListingDTO>
                {
                    Data = productionPlanListingDTOs,
                    PageCount = pageCount,
                    PageIndex = productionPlanPageRequest.PageIndex,
                    PageSize = productionPlanPageRequest.PageSize,
                    TotalItem = totalItem
                };
            return defaultPageResponseListingDTO;
        }

        private IQueryable<ProductionPlan> Filter(IQueryable<ProductionPlan> query, ProductionPlanPageRequest productionPlanPageRequest)
        {
            // Lọc theo Code
            if (!string.IsNullOrWhiteSpace(productionPlanPageRequest.Code))
            {
                var codeLower = productionPlanPageRequest.Code.ToLower();
                query = query.Where(pp => pp.Code.ToLower().Contains(codeLower));
            }

            if (!string.IsNullOrWhiteSpace(productionPlanPageRequest.Name))
            {
                var nameLower = productionPlanPageRequest.Name.ToLower();
                query = query.Where(pp => pp.Name.ToLower().Contains(nameLower));
            }

            if (productionPlanPageRequest.ExpectedStartingDate.HasValue)
            {
                var expectedStartingDate = productionPlanPageRequest.ExpectedStartingDate.Value.Date;
                query = query.Where(pp => pp.ExpectedStartingDate.Date == expectedStartingDate);
            }

            if (productionPlanPageRequest.DueDate.HasValue)
            {
                var dueDate = productionPlanPageRequest.DueDate.Value.Date;
                query = query.Where(pp => pp.DueDate.Date == dueDate);
            }

            if (!string.IsNullOrWhiteSpace(productionPlanPageRequest.Status) &&
                Enum.TryParse<ProductionPlanStatus>(productionPlanPageRequest.Status, true, out var parsedStatus))
            {
                query = query.Where(pp => pp.Status == parsedStatus);
            }

            if (!string.IsNullOrWhiteSpace(productionPlanPageRequest.Type)
                && Enum.TryParse<ProductionPlanType>(productionPlanPageRequest.Type, true, out var parsedType))
            {
                query = query
                .Where(productionPlan => productionPlan.Type == parsedType);
            }

            return query;
        }

    }
}
