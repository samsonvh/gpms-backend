using AutoMapper;
using FluentValidation;
using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Enums.Types;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
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
        private readonly IGenericRepository<ProductionRequirement> _productionRequirementRepository;
        private readonly IValidator<ProductionPlanInputDTO> _productionPlanValidator;
        private readonly IMapper _mapper;
        private readonly IProductionRequirementService _productionRequirementService;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly CurrentLoginUserDTO _currentLoginUser;
        private readonly IGenericRepository<Staff> _staffRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public ProductionPlanService(
                                    IGenericRepository<ProductionPlan> productionPlanRepository,
                                    IGenericRepository<ProductionRequirement> productionRequirementRepository,
                                    IValidator<ProductionPlanInputDTO> productionPlanValidator,
                                    IMapper mapper,
                                    IProductionRequirementService productionRequirementService,
                                    EntityListErrorWrapper entityListErrorWrapper,
                                    CurrentLoginUserDTO currentLoginUser,
                                    IGenericRepository<Staff> staffRepository,
                                    IGenericRepository<Product> productRepository)
        {
            _productionPlanRepository = productionPlanRepository;
            _productionRequirementRepository = productionRequirementRepository;
            _productionPlanValidator = productionPlanValidator;
            _mapper = mapper;
            _productionRequirementService = productionRequirementService;
            _entityListErrorWrapper = entityListErrorWrapper;
            _currentLoginUser = currentLoginUser;
            _staffRepository = staffRepository;
            _productRepository = productRepository;
        }

        public async Task<List<CreateUpdateResponseDTO<ProductionPlan>>> AddAnnualProductionPlanList
        (List<ProductionPlanInputDTO> inputDTOs)
        {

            ServiceUtils.ValidateInputDTOList<ProductionPlanInputDTO, ProductionPlan>
            (inputDTOs, _productionPlanValidator, _entityListErrorWrapper);
            await ServiceUtils.CheckFieldDuplicatedWithInputDTOListAndDatabase<ProductionPlanInputDTO, ProductionPlan>
                (inputDTOs, _productionPlanRepository, "Code", "Code", _entityListErrorWrapper);
            List<CreateUpdateResponseDTO<ProductionPlan>> productionPlanIdCodeList =
                await HandleAddAnnualProductionPlan(inputDTOs);

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
                await HandleAddChildProductionPlan(inputDTOs);
            Guid parentProductionPlanId = (Guid)inputDTOs.FirstOrDefault().ParentProductionPlanId;
            //CheckProductionPlanRequirementQuantityWithParentProductionPlan(inputDTOs,parentProductionPlanId);
            if (_entityListErrorWrapper.EntityListErrors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Create Production Plan Failed", _entityListErrorWrapper);
            }
            await _productionPlanRepository.Save();
            return productionPlanIdCodeList;
        }

        private async void CheckProductionPlanRequirementQuantityWithParentProductionPlan
            (List<ProductionPlanInputDTO> inputDTOs, Guid parentProductionPlanId)
        {

            ProductionPlan parentProductionPlan =
                await _productionPlanRepository
                    .Search(productionPlan => productionPlan.Id.Equals(parentProductionPlanId))
                    .Include(productionPlan => productionPlan.ProductionRequirements)
                        .ThenInclude(requirement => requirement.ProductionEstimations)
                    .FirstOrDefaultAsync();

            if (parentProductionPlan != null)
            {
                List<ProductionEstimation> parentEstimation =
                    parentProductionPlan.ProductionRequirements.SelectMany(requirement => requirement.ProductionEstimations).ToList();

            }
        }

        private async Task<List<CreateUpdateResponseDTO<ProductionPlan>>> HandleAddAnnualProductionPlan
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
                await _productionRequirementService.AddRequirementListForAnnualProductionPlan(inputDTO.ProductionRequirements, productionPlan.Id);
            }

            return productionPlanIdCodeList;
        }

        private async Task<List<CreateUpdateResponseDTO<ProductionPlan>>> HandleAddChildProductionPlan
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
                await _productionRequirementService.AddRequirementListForChildProductionPlan(inputDTO.ProductionRequirements, productionPlan.Id);
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

        public async Task<DefaultPageResponseListingDTO<ProductionPlanListingDTO>> GetAll(ProductionPlanFilterModel productionPlanFilterModel)
        {
            IQueryable<ProductionPlan> query = _productionPlanRepository.GetAll();
            query = Filter(query, productionPlanFilterModel);
            query = query.SortBy(productionPlanFilterModel);
            List<ProductionPlan> productionPlanList = await query.ToListAsync();
            int totalItem = productionPlanList.Count;
            productionPlanList = productionPlanList.PagingEntityList(productionPlanFilterModel);
            List<ProductionPlanListingDTO> productionPlanListingDTOs = new List<ProductionPlanListingDTO>();
            foreach (ProductionPlan productionPlan in productionPlanList)
            {
                ProductionPlanListingDTO productionPlanListingDTO = _mapper.Map<ProductionPlanListingDTO>(productionPlan);
                productionPlanListingDTOs.Add(productionPlanListingDTO);
            }

            DefaultPageResponseListingDTO<ProductionPlanListingDTO> defaultPageResponseListingDTO =
                new DefaultPageResponseListingDTO<ProductionPlanListingDTO>
                {
                    Data = productionPlanListingDTOs,
                    Pagination = new PaginationResponseModel
                    {
                        PageIndex = productionPlanFilterModel.Pagination.PageIndex,
                        PageSize = productionPlanFilterModel.Pagination.PageSize,
                        TotalRows = totalItem
                    }
                };
            return defaultPageResponseListingDTO;
        }

        private IQueryable<ProductionPlan> Filter(IQueryable<ProductionPlan> query, ProductionPlanFilterModel productionPlanPageRequest)
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

        private async Task<ProductionPlan> GetProductionPlanById(Guid id)
        {
            return await _productionPlanRepository
                .Search(productionPlan => productionPlan.Id == id)
                .Include(productionPlan => productionPlan.Creator)
                .Include(productionPlan => productionPlan.ProductionRequirements)
                    .ThenInclude(productionRequirement => productionRequirement.ProductSpecification)
                        .ThenInclude(productSpecification => productSpecification.Product)
                .FirstOrDefaultAsync();
        }

        public async Task<ChangeStatusResponseDTO<ProductionPlan, ProductionPlanStatus>> ChangeStatus
            (Guid id, string productionPlanStatus)
        {
            var productionPlan = await GetProductionPlanById(id);

            if (productionPlan == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Production plan not found");
            }

            var parsedStatus = ValidateProductionPlanStatus(productionPlanStatus, productionPlan);

            if (parsedStatus == ProductionPlanStatus.Approved && productionPlan.Status == ProductionPlanStatus.Pending)
            {
                await ApproveProductionPlan(productionPlan);
            }

            if (parsedStatus == ProductionPlanStatus.InProgress && productionPlan.Status == ProductionPlanStatus.Approved)
            {
                await ChangeStatusToInProgress(productionPlan);
            }

            productionPlan.Status = parsedStatus;
            await _productionPlanRepository.Save();

            return _mapper.Map<ChangeStatusResponseDTO<ProductionPlan, ProductionPlanStatus>>(productionPlan);
        }

        private ProductionPlanStatus ValidateProductionPlanStatus(string productionPlanStatus, ProductionPlan productionPlan)
        {
            if (!Enum.TryParse(productionPlanStatus, true, out ProductionPlanStatus parsedStatus))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Invalid status value provided.");
            }

            switch (productionPlan.Status)
            {
                case ProductionPlanStatus.Pending:
                    if (parsedStatus == ProductionPlanStatus.Finished || parsedStatus == ProductionPlanStatus.InProgress)
                        throw new APIException((int)HttpStatusCode.BadRequest, $"Cannot change status from {ProductionPlanStatus.Pending} to {parsedStatus}.");
                    break;

                case ProductionPlanStatus.Approved:
                    if (parsedStatus == ProductionPlanStatus.Pending || parsedStatus == ProductionPlanStatus.Declined || parsedStatus == ProductionPlanStatus.Finished)
                        throw new APIException((int)HttpStatusCode.BadRequest, $"Cannot change status from {ProductionPlanStatus.Approved} to {parsedStatus}.");
                    break;

                case ProductionPlanStatus.Declined:
                    if (parsedStatus == ProductionPlanStatus.Pending || parsedStatus == ProductionPlanStatus.Approved || parsedStatus == ProductionPlanStatus.InProgress || parsedStatus == ProductionPlanStatus.Finished)
                        throw new APIException((int)HttpStatusCode.BadRequest, $"Cannot change status from {ProductionPlanStatus.Declined} to {parsedStatus}.");
                    break;
            }

            return parsedStatus;
        }

        private async Task ApproveProductionPlan(ProductionPlan productionPlan)
        {
            if (productionPlan.Creator != null)
            {
                productionPlan.Creator.Status = StaffStatus.In_production;
                await _staffRepository.Save();
            }

            productionPlan.ReviewerId = _currentLoginUser.StaffId;
        }

        private async Task ChangeStatusToInProgress(ProductionPlan productionPlan)
        {
            foreach (var requirement in productionPlan.ProductionRequirements)
            {
                var productSpecification = requirement.ProductSpecification;
                var product = productSpecification.Product;

                product.Status = ProductStatus.InProduction;
                await _productRepository.Save();
            }
        }

        #region Start Production Plan
        public async Task<ChangeStatusResponseDTO<ProductionPlan, ProductionPlanStatus>> StartProductionPlan(Guid id)
        {
            var productionPlan = await _productionPlanRepository.Search(productionPlan => productionPlan.Id.Equals(id))
                                        .Include(productionPlan => productionPlan.ProductionRequirements)
                                            .ThenInclude(requirement => requirement.ProductSpecification)
                                                .ThenInclude(specification => specification.Product)
                                        .FirstOrDefaultAsync();
            ValidateProductionPlanForStart(productionPlan);
            await HandleStartProductionPlan(productionPlan);
            return new ChangeStatusResponseDTO<ProductionPlan, ProductionPlanStatus>
            {
                Id = productionPlan.Id,
                Status = productionPlan.Status.ToString()
            };
        }

        private async Task HandleStartProductionPlan(ProductionPlan productionPlan)
        {
            List<Product> products = productionPlan.ProductionRequirements
                                .Select(requirement => requirement.ProductSpecification.Product).ToList();
            foreach (Product product in products)
            {
                if (product.Status.Equals(ProductStatus.Approved))
                {
                    product.Status = ProductStatus.InProduction;
                    _productRepository.Update(product);
                }
            }
            productionPlan.Status = ProductionPlanStatus.InProgress;
            _productionPlanRepository.Update(productionPlan);
            await _productRepository.Save();
        }

        private void ValidateProductionPlanForStart(ProductionPlan productionPlan)
        {
            if (productionPlan == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Production plan not found.");
            }
            if (!productionPlan.Status.Equals(ProductionPlanStatus.Approved))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Cannot start production plan with a status other than approved.");
            }
            if (_currentLoginUser.StaffId != productionPlan.CreatorId)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Only the creator can start the production plan.");
            }
            if (!productionPlan.Type.Equals(ProductionPlanType.Batch))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Can only start production plan with Type set to Batch.");
            }
        }
        #endregion 
    }
}
