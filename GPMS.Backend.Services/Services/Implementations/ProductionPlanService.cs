using AutoMapper;
using FluentValidation;
using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Data.Enums.Types;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.Specifications;
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
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly CurrentLoginUserDTO _currentLoginUser;

        public ProductionPlanService(IGenericRepository<ProductionPlan> productionPlanRepository,
                                    IValidator<ProductionPlanInputDTO> productionPlanValidator,
                                    IMapper mapper,
                                    IProductionRequirementService productionRequirementService,
                                    EntityListErrorWrapper entityListErrorWrapper,
                                    CurrentLoginUserDTO currentLoginUser)
        {
            _productionPlanRepository = productionPlanRepository;
            _productionPlanValidator = productionPlanValidator;
            _mapper = mapper;
            _productionRequirementService = productionRequirementService;
            _entityListErrorWrapper = entityListErrorWrapper;
            _currentLoginUser = currentLoginUser;
        }

        public async Task<List<CreateUpdateResponseDTO<ProductionPlan>>> AddAnnualProductionPlanList
        (List<ProductionPlanInputDTO> inputDTOs)
        {

            ServiceUtils.ValidateInputDTOList<ProductionPlanInputDTO, ProductionPlan>
            (inputDTOs, _productionPlanValidator, _entityListErrorWrapper);
            await ServiceUtils.CheckFieldDuplicatedWithInputDTOListAndDatabase<ProductionPlanInputDTO, ProductionPlan>
                (inputDTOs, _productionPlanRepository, "Code", "Code", _entityListErrorWrapper);
            List<CreateUpdateResponseDTO<ProductionPlan>> productionPlanIdCodeList =
                await HandleAddProductionPlan(inputDTOs);

            if (_entityListErrorWrapper.EntityListErrors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Create Production Plan List Failed", _entityListErrorWrapper);
            }
            await _productionPlanRepository.Save();
            return productionPlanIdCodeList;
        }

        public async Task<List<CreateUpdateResponseDTO<ProductionPlan>>> AddChildProductionPlanList(List<ProductionPlanInputDTO> inputDTOs)
        {
            ServiceUtils.ValidateInputDTOList<ProductionPlanInputDTO, ProductionPlan>
            (inputDTOs, _productionPlanValidator, _entityListErrorWrapper);
            await ServiceUtils.CheckFieldDuplicatedWithInputDTOListAndDatabase<ProductionPlanInputDTO, ProductionPlan>
                (inputDTOs, _productionPlanRepository, "Code", "Code", _entityListErrorWrapper);
            List<CreateUpdateResponseDTO<ProductionPlan>> productionPlanIdCodeList =
                await HandleAddProductionPlan(inputDTOs);

            if (_entityListErrorWrapper.EntityListErrors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Create Production Plan Failed", _entityListErrorWrapper);
            }
            await _productionPlanRepository.Save();
            return productionPlanIdCodeList;
        }

        private async Task<List<CreateUpdateResponseDTO<ProductionPlan>>> HandleAddProductionPlan
        (List<ProductionPlanInputDTO> inputDTOs)
        {
            List<CreateUpdateResponseDTO<ProductionPlan>> productionPlanIdCodeList = new List<CreateUpdateResponseDTO<ProductionPlan>>();
            foreach (ProductionPlanInputDTO inputDTO in inputDTOs)
            {
                ProductionPlan productionPlan = _mapper.Map<ProductionPlan>(inputDTO);
                productionPlan.CreatorId = _currentLoginUser.StaffId;
                productionPlan.Type = (ProductionPlanType)Enum.Parse(typeof(ProductionPlanType), inputDTO.Type);
                productionPlan.Status = ProductionPlanStatus.Pending;
                _productionPlanRepository.Add(productionPlan);
                CreateUpdateResponseDTO<ProductionPlan> productionPlanIdCode = new CreateUpdateResponseDTO<ProductionPlan>
                {
                    Id = productionPlan.Id,
                    Code = productionPlan.Code,
                };
                productionPlanIdCodeList.Add(productionPlanIdCode);
                await _productionRequirementService.AddList(inputDTO.ProductionRequirements, productionPlan.Id);
            }

            return productionPlanIdCodeList;
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
                .Include(productionPlan => productionPlan.ParentProductionPlan)
                .Include(productionPlan => productionPlan.ChildProductionPlans)
                .Include(productionPlan => productionPlan.ProductionRequirements)
                    .ThenInclude(productionRequirement => productionRequirement.ProductSpecification)
                        .ThenInclude(productSpecification => productSpecification.Product)
                .Include(productionPlan => productionPlan.ProductionRequirements)
                    .ThenInclude(productionRequirement => productionRequirement.ProductionEstimations)
                        .ThenInclude(productionEstimation => productionEstimation.ProductionSeries)
                .Include(productionPlan => productionPlan.Creator)
                .Include(productionPlan => productionPlan.Reviewer)
                .Include(productionPlan => productionPlan.ParentProductionPlan.Creator)
                .Include(productionPlan => productionPlan.ParentProductionPlan.Reviewer)
                .Include(productionPlan => productionPlan.ChildProductionPlans)
                    .ThenInclude(child => child.Creator)
                .Include(productionPlan => productionPlan.ChildProductionPlans)
                    .ThenInclude(child => child.Reviewer)
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            if (productionPlan == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, $"Production Plan with ID: {id} not found");
            }

            ProductionPlanDTO productionPlanDTO = _mapper.Map<ProductionPlanDTO>(productionPlan);

            foreach (var requirement in productionPlanDTO.ProductionRequirements)
            {
                var productionRequirement = productionPlan.ProductionRequirements
                    .FirstOrDefault(r => r.Id == requirement.Id);

                if (productionRequirement != null)
                {
                    requirement.ProductionEstimations = productionRequirement.ProductionEstimations
                        .Select(e => _mapper.Map<ProductionEstimationDTO>(e))
                        .ToList();
                }
            }

            foreach (var requirement in productionPlanDTO.ProductionRequirements)
            {
                requirement.ProductSpecification.Measurements = null;
                requirement.ProductSpecification.BillOfMaterials = null;
                requirement.ProductSpecification.QualityStandards = null;
            }
            return MapParentAndChildProductionPlan(productionPlanDTO, productionPlan);
        }

        private ProductionPlanDTO MapParentAndChildProductionPlan(ProductionPlanDTO productionPlanDTO, ProductionPlan productionPlan)
        {


            if (productionPlan.ParentProductionPlan != null)
            {
                productionPlanDTO.ParentProductionPlan = new ProductionPlanDTO();
                productionPlanDTO.ParentProductionPlan.Id = productionPlan.ParentProductionPlan.Id;
                productionPlanDTO.ParentProductionPlan.Code = productionPlan.ParentProductionPlan.Code;
                productionPlanDTO.ParentProductionPlan.Name = productionPlan.ParentProductionPlan.Name;
                productionPlanDTO.ParentProductionPlan.Description = productionPlan.ParentProductionPlan.Description;
                productionPlanDTO.ParentProductionPlan.ExpectedStartingDate = productionPlan.ParentProductionPlan.ExpectedStartingDate;
                productionPlanDTO.ParentProductionPlan.DueDate = productionPlan.ParentProductionPlan.DueDate;
                productionPlanDTO.ParentProductionPlan.ActualStartingDate = productionPlan.ParentProductionPlan.ActualStartingDate;
                productionPlanDTO.ParentProductionPlan.CompletionDate = productionPlan.ParentProductionPlan.CompletionDate;
                productionPlanDTO.ParentProductionPlan.Type = productionPlan.ParentProductionPlan.Type.ToString();
                productionPlanDTO.ParentProductionPlan.CreatedDate = productionPlan.ParentProductionPlan.CreatedDate;
                productionPlanDTO.ParentProductionPlan.Status = productionPlan.ParentProductionPlan.Status.ToString();

                productionPlanDTO.ParentProductionPlan.CreatorName = productionPlan.ParentProductionPlan.Creator.FullName;
                if (productionPlan.ParentProductionPlan.Reviewer != null)
                    productionPlanDTO.ParentProductionPlan.ReviewerName = productionPlan.ParentProductionPlan.Reviewer.FullName;
            }

            if (productionPlan.ChildProductionPlans.Count > 0)
            {
                List<ProductionPlanDTO> childProductionPlans = new List<ProductionPlanDTO>();
                foreach (ProductionPlan childProductionPlan in productionPlan.ChildProductionPlans)
                {
                    ProductionPlanDTO childProductionPlanDTO = new ProductionPlanDTO();
                    childProductionPlanDTO.Id = childProductionPlan.Id;
                    childProductionPlanDTO.Code = childProductionPlan.Code;
                    childProductionPlanDTO.Name = childProductionPlan.Name;
                    childProductionPlanDTO.Description = childProductionPlan.Description;
                    childProductionPlanDTO.ExpectedStartingDate = childProductionPlan.ExpectedStartingDate;
                    childProductionPlanDTO.DueDate = childProductionPlan.DueDate;
                    childProductionPlanDTO.ActualStartingDate = childProductionPlan.ActualStartingDate;
                    childProductionPlanDTO.CompletionDate = childProductionPlan.CompletionDate;
                    childProductionPlanDTO.Type = childProductionPlan.Type.ToString();
                    childProductionPlanDTO.CreatedDate = childProductionPlan.CreatedDate;
                    childProductionPlanDTO.Status = childProductionPlan.Status.ToString();
                    childProductionPlanDTO.CreatorName = childProductionPlan.Creator.FullName;
                    if (childProductionPlan.Reviewer != null)
                        childProductionPlanDTO.ParentProductionPlan.ReviewerName = childProductionPlan.Reviewer.FullName;
                    childProductionPlans.Add(childProductionPlanDTO);
                }
                productionPlanDTO.ChildProductionPlans = childProductionPlans;
            }
            return productionPlanDTO;
        }


        public Task<CreateUpdateResponseDTO<ProductionPlan>> Add(ProductionPlanInputDTO inputDTO)
        {
            throw new NotImplementedException();
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
