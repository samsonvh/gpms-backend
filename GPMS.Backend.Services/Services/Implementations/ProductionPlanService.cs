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
        private readonly IValidator<ProductionPlanInputDTO> _productionPlanValidator;
        private readonly IMapper _mapper;
        private readonly IProductionRequirementService _productionRequirementService;
        private readonly IProductionEstimationService _productionEstimationService;
        private readonly IProductionSeriesService _productionSeriesService;

        public ProductionPlanService(IGenericRepository<ProductionPlan> productionPlanRepository,
                                    IValidator<ProductionPlanInputDTO> productionPlanValidator,
                                    IMapper mapper,
                                    IProductionRequirementService productionRequirementService,
                                    IProductionEstimationService productionEstimationService,
                                    IProductionSeriesService productionSeriesService)
        {
            _productionPlanRepository = productionPlanRepository;
            _productionPlanValidator = productionPlanValidator;
            _mapper = mapper;
            _productionRequirementService = productionRequirementService;
            _productionEstimationService = productionEstimationService;
            _productionSeriesService = productionSeriesService;
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

            ServiceUtils.ValidateInputDTO<ProductionPlanInputDTO, ProductionPlan>(inputDTO, _productionPlanValidator);

              await ServiceUtils.CheckFieldDuplicatedWithInputDTOAndDatabase<ProductionPlanInputDTO, ProductionPlan>(
                inputDTO, _productionPlanRepository, "Code", "Code");

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
    }
}
