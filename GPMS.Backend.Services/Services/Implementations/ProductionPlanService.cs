using AutoMapper;
using FluentValidation;
using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
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
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            if (productionPlan == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, $"Production Plan with ID: {id} not found");
            }

            return MapToProductionPlanDTO(productionPlan);
        }

        private ProductionPlanDTO MapToProductionPlanDTO(ProductionPlan productionPlan)
        {
            var productionPlanDto = _mapper.Map<ProductionPlanDTO>(productionPlan);

            if (productionPlan.Type == ProductionPlanType.Batch)
            {
                productionPlanDto.ChildProductionPlans = null;
            }
            else
            {
                productionPlanDto.ChildProductionPlans = productionPlan.ChildProductionPlans
                    .Select(child => _mapper.Map<ChildProductionPlanDTO>(child))
                    .ToList();
            }

            if (productionPlan.ParentProductionPlan != null)
            {
                productionPlanDto.ParentProductionPlan = _mapper.Map<ParentProductionPlanDTO>(productionPlan.ParentProductionPlan);
            }

            productionPlanDto.ProductionRequirements = productionPlan.ProductionRequirements
                .Select(requirement =>
                {
                    var requirementDto = _mapper.Map<ProductionRequirementDTO>(requirement);
                    requirementDto.ProductSpecification.Product = new ProductDTO
                    {
                        Id = requirement.ProductSpecification.Product.Id,
                        Code = requirement.ProductSpecification.Product.Code,
                        Name = requirement.ProductSpecification.Product.Name,
                        Status = requirement.ProductSpecification.Product.Status.ToString()
                    };
                    return requirementDto;
                }).ToList();

            return productionPlanDto;
        }

        public Task<CreateUpdateResponseDTO<ProductionPlan>> Add(ProductionPlanInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }
    }
}
