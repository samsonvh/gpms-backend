using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        #region Add Annual 
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

        private async Task<List<CreateUpdateResponseDTO<ProductionPlan>>> HandleAddAnnualProductionPlan
            (List<ProductionPlanInputDTO> inputDTOs)
        {
            List<CreateUpdateResponseDTO<ProductionPlan>> productionPlanIdCodeList = new List<CreateUpdateResponseDTO<ProductionPlan>>();
            foreach (ProductionPlanInputDTO inputDTO in inputDTOs)
            {
                ProductionPlan productionPlan = _mapper.Map<ProductionPlan>(inputDTO);
                productionPlan.CreatorId = _currentLoginUser.Id;
                productionPlan.Type = (ProductionPlanType)Enum.Parse(typeof(ProductionPlanType), inputDTO.Type);
                productionPlan.Status = ProductionPlanStatus.Approved;
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
        #endregion
        #region Add Child 
        public async Task<List<CreateUpdateResponseDTO<ProductionPlan>>> AddChildProductionPlanList(List<ProductionPlanInputDTO> inputDTOs)
        {
            ServiceUtils.ValidateInputDTOList<ProductionPlanInputDTO, ProductionPlan>
            (inputDTOs, _productionPlanValidator, _entityListErrorWrapper);
            await ServiceUtils.CheckFieldDuplicatedWithInputDTOListAndDatabase<ProductionPlanInputDTO, ProductionPlan>
                (inputDTOs, _productionPlanRepository, "Code", "Code", _entityListErrorWrapper);
            List<CreateUpdateResponseDTO<ProductionPlan>> productionPlanIdCodeList =
                await HandleAddChildProductionPlan(inputDTOs);
            Guid parentProductionPlanId = (Guid)inputDTOs.FirstOrDefault().ParentProductionPlanId;
            CheckProductionPlanRequirementQuantityWithParentProductionPlan(inputDTOs, parentProductionPlanId);
            if (_entityListErrorWrapper.EntityListErrors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Create Production Plan Failed", _entityListErrorWrapper);
            }
            await _productionPlanRepository.Save();
            return productionPlanIdCodeList;
        }

        private async Task<List<CreateUpdateResponseDTO<ProductionPlan>>> HandleAddChildProductionPlan
        (List<ProductionPlanInputDTO> inputDTOs)
        {
            List<CreateUpdateResponseDTO<ProductionPlan>> productionPlanIdCodeList = new List<CreateUpdateResponseDTO<ProductionPlan>>();
            foreach (ProductionPlanInputDTO inputDTO in inputDTOs)
            {
                ProductionPlan productionPlan = _mapper.Map<ProductionPlan>(inputDTO);
                productionPlan.CreatorId = _currentLoginUser.Id;
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

        private async void CheckProductionPlanRequirementQuantityWithParentProductionPlan
            (List<ProductionPlanInputDTO> inputDTOs, Guid parentProductionPlanId)
        {

            ProductionPlan parentProductionPlan =
                await _productionPlanRepository
                    .Search(productionPlan => productionPlan.Id.Equals(parentProductionPlanId))
                    .Include(productionPlan => productionPlan.ProductionRequirements)
                    .FirstOrDefaultAsync();

            if (parentProductionPlan != null)
            {
                var requirementsInputDTOs = inputDTOs.SelectMany(inputDTO => inputDTO.ProductionRequirements).ToList();
                foreach (ProductionRequirement parentRequirement in parentProductionPlan.ProductionRequirements)
                {
                    // var requirementQuantityWithSum
                }
            }
        }

        #endregion
        public Task AddList(List<ProductionPlanInputDTO> inputDTOs, Guid? parentEntityId = null)
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


        #region Get All Production Plan
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
        #endregion
        #region Get All Child 
        public async Task<DefaultPageResponseListingDTO<ProductionPlanListingDTO>> GetAllChildByParentId(ProductionPlanFilterModel productionPlanFilterModel, Guid parentId)
        {
            IQueryable<ProductionPlan> query =
                _productionPlanRepository
                .Search(productionPlan => productionPlan.ParentProductionPlanId.Equals(parentId));
            query = Filter(query, productionPlanFilterModel);
            query = query.SortBy<ProductionPlan>(productionPlanFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery<ProductionPlan>(productionPlanFilterModel);
            var data = await query.ProjectTo<ProductionPlanListingDTO>(_mapper.ConfigurationProvider)
                                    .ToListAsync();
            return new DefaultPageResponseListingDTO<ProductionPlanListingDTO>
            {
                Data = data,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = productionPlanFilterModel.Pagination.PageIndex,
                    PageSize = productionPlanFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }
        #endregion

        private async Task<ProductionPlan> GetProductionPlanById(Guid id)
        {
            return await _productionPlanRepository
                .Search(productionPlan => productionPlan.Id == id)
                .Include(productionPlan => productionPlan.ParentProductionPlan)
                    .ThenInclude(parent => parent.ChildProductionPlans)
                .Include(productionPlan => productionPlan.ChildProductionPlans)
                .Include(productionPlan => productionPlan.Creator)
                .Include(productionPlan => productionPlan.ProductionRequirements)
                    .ThenInclude(productionRequirement => productionRequirement.ProductSpecification)
                        .ThenInclude(productSpecification => productSpecification.Product)
                .FirstOrDefaultAsync();
        }

        #region Approve Production Plan
        public async Task<ChangeStatusResponseDTO<ProductionPlan, ProductionPlanStatus>> Approve
            (Guid id)
        {
            var productionPlan = await GetProductionPlanById(id);
            ValidateForApproveProductionPlan(productionPlan);
            productionPlan.Creator.Status = StaffStatus.In_production;
            _staffRepository.Update(productionPlan.Creator);
            productionPlan.ReviewerId = _currentLoginUser.Id;
            productionPlan.Status = ProductionPlanStatus.Approved;
            _productionPlanRepository.Update(productionPlan);
            await _productionPlanRepository.Save();
            return _mapper.Map<ChangeStatusResponseDTO<ProductionPlan, ProductionPlanStatus>>(productionPlan);
        }

        private void ValidateForApproveProductionPlan(ProductionPlan productionPlan)
        {
            if (productionPlan == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Production plan not found");
            }
            if (!productionPlan.Status.Equals(ProductionPlanStatus.Pending))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Cannot approve Production Plan with a Status is not Pending.");
            }
            if (productionPlan.Type.Equals(ProductionPlanType.Year))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Can only approve Production Plan with Month/Batch Type.");
            }
            if (productionPlan.Type.Equals(ProductionPlanType.Month))
            {
                int childApprove = 0;
                foreach (ProductionPlan childProductionPlan in productionPlan.ChildProductionPlans)
                {
                    if (childProductionPlan.Status.Equals(ProductionPlanStatus.Approved))
                    {
                        childApprove++;
                    }
                }
                if (childApprove < productionPlan.ChildProductionPlans.Count)
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Must Approve full Batch Production Plan before approving Month Production Plan");
                }
            }
        }
        #endregion
        #region Decline Production Plan
        public async Task<ChangeStatusResponseDTO<ProductionPlan, ProductionPlanStatus>> Decline
            (Guid id)
        {
            var productionPlan = await GetProductionPlanById(id);
            ValidateForDeclineProductionPlan(productionPlan);
            HandleDeclineProductionPlan(productionPlan);
            productionPlan.ReviewerId = _currentLoginUser.Id;
            _productionPlanRepository.Update(productionPlan);
            await _productionPlanRepository.Save();
            return _mapper.Map<ChangeStatusResponseDTO<ProductionPlan, ProductionPlanStatus>>(productionPlan);
        }

        private void ValidateForDeclineProductionPlan(ProductionPlan productionPlan)
        {
            if (productionPlan == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Production plan not found");
            }
            if (!productionPlan.Status.Equals(ProductionPlanStatus.Pending))
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Cannot decline Production Plan with a Status is not Pending.");
            }
            if (productionPlan.Type.Equals(ProductionPlanType.Year))
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Can only decline Production Plan with Month/Batch Type.");
            }
        }

        private void HandleDeclineProductionPlan(ProductionPlan productionPlan)
        {
            if (productionPlan.Type.Equals(ProductionPlanType.Month))
            {
                if (productionPlan.ChildProductionPlans.Count == 0)
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Can not decline Month Production Plan don't have any Batch Production Plan");
                }
                foreach (ProductionPlan childProductionPlan in productionPlan.ChildProductionPlans)
                {
                    childProductionPlan.Status = ProductionPlanStatus.Declined;
                    childProductionPlan.ReviewerId = _currentLoginUser.Id;
                    _productionPlanRepository.Update(childProductionPlan);
                }
                productionPlan.ReviewerId = _currentLoginUser.Id;
                productionPlan.Status = ProductionPlanStatus.Declined;
                _productionPlanRepository.Update(productionPlan);
            }
            else
            {
                int declineCount = 0;
                if (productionPlan.ParentProductionPlan == null)
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Can not decline Batch Production Plan don't have Month Production Plan");
                }
                foreach (ProductionPlan childOfParent in productionPlan.ParentProductionPlan.ChildProductionPlans)
                {
                    if (childOfParent.Status.Equals(ProductionPlanStatus.Declined))
                    {
                        declineCount++;
                    }
                }
                if (declineCount == productionPlan.ParentProductionPlan.ChildProductionPlans.Count - 1)
                {
                    productionPlan.ParentProductionPlan.ReviewerId = _currentLoginUser.Id;
                    productionPlan.ParentProductionPlan.Status = ProductionPlanStatus.Declined;
                }
                productionPlan.ReviewerId = _currentLoginUser.Id;
                productionPlan.Status = ProductionPlanStatus.Declined;
                _productionPlanRepository.Update(productionPlan);
            }
        }

        #endregion
        #region Start Production Plan
        public async Task<ChangeStatusResponseDTO<ProductionPlan, ProductionPlanStatus>> StartProductionPlan(Guid id)
        {
            var productionPlan = await _productionPlanRepository.Search(productionPlan => productionPlan.Id.Equals(id))
                                        .Include(productionPlan => productionPlan.ProductionRequirements)
                                            .ThenInclude(requirement => requirement.ProductSpecification)
                                                .ThenInclude(specification => specification.Product)
                                                    .ThenInclude(product => product.ProductionProcesses)
                                        .Include(productionPlan => productionPlan.ProductionRequirements)
                                            .ThenInclude(requirement => requirement.ProductionEstimations)
                                                .ThenInclude(estimation => estimation.ProductionSeries)
                                        .Include(productionPlan => productionPlan.ParentProductionPlan)
                                            .ThenInclude(productionPlan => productionPlan.ParentProductionPlan)
                                        .FirstOrDefaultAsync();
            ValidateProductionPlanForStart(productionPlan);
            await HandleStartProductionPlan(productionPlan);
            return new ChangeStatusResponseDTO<ProductionPlan, ProductionPlanStatus>
            {
                Id = productionPlan.Id,
                Status = productionPlan.Status.ToString()
            };
        }

        private void ValidateProductionPlanForStart(ProductionPlan productionPlan)
        {
            if (productionPlan == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Batch Production Plan not found.");
            }
            if (!productionPlan.Status.Equals(ProductionPlanStatus.Approved))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Cannot start Batch Production Plan with a Status is not Approved.");
            }
            if (_currentLoginUser.Id != productionPlan.CreatorId)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Only the Creator can start the Batch Production Plan.");
            }
            if (!productionPlan.Type.Equals(ProductionPlanType.Batch))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Can only start Production Plan with Production Plan Type set to Batch.");
            }
            if (productionPlan.ParentProductionPlan == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Cannot start Batch Production Plan don't have Month Production Plan.");
            }
            else
            {
                if (!productionPlan.ParentProductionPlan.Status.Equals(ProductionPlanStatus.Approved)
                    && !productionPlan.ParentProductionPlan.Status.Equals(ProductionPlanStatus.InProgress))
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Can not start Batch Production Plan because Month Production Plan Status of this Batch is not approve or in progress");
                }
                if (productionPlan.ParentProductionPlan.ParentProductionPlan == null)
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Cannot start Batch Production Plan don't have Year Production Plan.");
                }
                else if (!productionPlan.ParentProductionPlan.ParentProductionPlan.Status.Equals(ProductionPlanStatus.Approved)
                && !productionPlan.ParentProductionPlan.ParentProductionPlan.Status.Equals(ProductionPlanStatus.InProgress))
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Can not start Batch Production Plan because Year Production Plan Status of this Batch is not approve or in progress");
                }
            }
        }
        private async Task HandleStartProductionPlan(ProductionPlan productionPlan)
        {
            //Chuyển status của month và year sang InProgress
            if (productionPlan.ParentProductionPlan.ParentProductionPlan.Status.Equals(ProductionPlanStatus.Approved))
                productionPlan.ParentProductionPlan.ParentProductionPlan.Status = ProductionPlanStatus.InProgress;
            if (productionPlan.ParentProductionPlan.Status.Equals(ProductionPlanStatus.Approved))
                productionPlan.ParentProductionPlan.Status = ProductionPlanStatus.InProgress;

            List<Product> products = productionPlan.ProductionRequirements
                                .Select(requirement => requirement.ProductSpecification.Product).ToList();
            //Chuyển status của product sang approve
            foreach (Product product in products)
            {
                if (product.Status.Equals(ProductStatus.Approved))
                {
                    product.Status = ProductStatus.InProduction;
                    _productRepository.Update(product);
                }
            }
            //Chuyển current process của các Series thuộc estimation đầu tiên sang process đầu tiên của product 
            foreach (var requirement in productionPlan.ProductionRequirements)
            {
                var seriesOfFirstEstimation = productionPlan.ProductionRequirements
                    .SelectMany(requirement => requirement.ProductionEstimations)
                    .FirstOrDefault(estimation => estimation.DayNumber == 1)
                    .ProductionSeries
                    .ToList();
                foreach (var series in seriesOfFirstEstimation)
                {
                    series.CurrentProcess = requirement.ProductSpecification.Product
                    .ProductionProcesses.FirstOrDefault(process => process.OrderNumber == 1).Name;
                    series.Status = ProductionSeriesStatus.InProduction;
                }
            }
            productionPlan.Status = ProductionPlanStatus.InProgress;
            productionPlan.ActualStartingDate = DateTime.UtcNow;
            _productionPlanRepository.Update(productionPlan);
            await _productRepository.Save();
        }
        #endregion

        public Task<ProductionPlanDTO> Add(ProductionPlanInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ProductionPlanDTO> Update(Guid id, ProductionPlanInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }


    }
}
