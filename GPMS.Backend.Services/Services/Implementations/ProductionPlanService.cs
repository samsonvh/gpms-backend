using AutoMapper;
using FluentValidation;
using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
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
using GPMS.Backend.Services.Utils;
using GPMS.Backend.Services.Utils.Validators;
using Microsoft.EntityFrameworkCore;
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

        public ProductionPlanService(
                                    IGenericRepository<ProductionPlan> productionPlanRepository,
                                    IGenericRepository<ProductionRequirement> productionRequirementRepository,
                                    IValidator<ProductionPlanInputDTO> productionPlanValidator,
                                    IMapper mapper,
                                    IProductionRequirementService productionRequirementService,
                                    EntityListErrorWrapper entityListErrorWrapper,
                                    CurrentLoginUserDTO currentLoginUser)
        {
            _productionPlanRepository = productionPlanRepository;
            _productionRequirementRepository = productionRequirementRepository;
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

        public Task<CreateUpdateResponseDTO<ProductionPlan>> Add(ProductionPlanInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }
    }
}
