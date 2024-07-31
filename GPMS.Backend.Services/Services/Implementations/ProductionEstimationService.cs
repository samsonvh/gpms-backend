using AutoMapper;
using FluentValidation;
using GPMS.Backend.Data.Enums.Times;
using GPMS.Backend.Data.Enums.Types;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Utils;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class ProductionEstimationService : IProductionEstimationService
    {
        private readonly IGenericRepository<ProductionEstimation> _productionEstimationRepository;
        private readonly IGenericRepository<ProductionPlan> _productionPlanRepository;
        private readonly IGenericRepository<ProductionRequirement> _productionRequirementRepository;
        private readonly IValidator<ProductionEstimationInputDTO> _productionEstimationValidator;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly IMapper _mapper;
        private readonly IProductionSeriesService _productionSeriesService;

        public ProductionEstimationService(
            IMapper mapper,
            IGenericRepository<ProductionEstimation> productionEstimationRepository,
            IGenericRepository<ProductionPlan> productionPlanRepository,
            IGenericRepository<ProductionRequirement> productionRequirementRepository,
            IValidator<ProductionEstimationInputDTO> productionEstimationValidator,
            EntityListErrorWrapper entityListErrorWrapper,
            IProductionSeriesService productionSeriesService)
        {
            _mapper = mapper;
            _productionEstimationRepository = productionEstimationRepository;
            _productionPlanRepository = productionPlanRepository;
            _productionRequirementRepository = productionRequirementRepository;
            _productionEstimationValidator = productionEstimationValidator;
            _entityListErrorWrapper = entityListErrorWrapper;
            _productionSeriesService = productionSeriesService;
        }

        public async Task AddEstimationListForAnnualProductionPlan
        (List<ProductionEstimationInputDTO> inputDTOs, Guid productionRequirementId, Guid productionPlanId)
        {
            ServiceUtils.ValidateInputDTOList<ProductionEstimationInputDTO, ProductionEstimation>
                (inputDTOs, _productionEstimationValidator, _entityListErrorWrapper);
            ProductionPlan productionPlan = _productionPlanRepository.GetUnAddedEntityById(productionPlanId);
            ProductionRequirement productionRequirement = _productionRequirementRepository.GetUnAddedEntityById(productionRequirementId);
            CheckTotalEstimationQuantityWithRequirementEntity(inputDTOs, productionRequirement);
            ValidateTimeInEstimationInputDTO(inputDTOs, productionPlan);
            foreach (ProductionEstimationInputDTO inputDTO in inputDTOs)
            {
                CheckSeriesExistInEstimation(inputDTO, inputDTOs.IndexOf(inputDTO) + 1, productionPlan);
                ProductionEstimation productionEstimation = _mapper.Map<ProductionEstimation>(inputDTO);
                productionEstimation.ProductionRequirementId = productionRequirementId;
                _productionEstimationRepository.Add(productionEstimation);
                if (productionPlan.Type.Equals(ProductionPlanType.Batch))
                {
                    await _productionSeriesService.AddList(inputDTO.ProductionSeries, productionEstimation.Id);
                }
            }
        }

        public async Task AddEstimationListForChildProductionPlan
            (List<ProductionEstimationInputDTO> inputDTOs, Guid productionRequirementId, Guid productionPlanId)
        {
            ServiceUtils.ValidateInputDTOList<ProductionEstimationInputDTO, ProductionEstimation>
                (inputDTOs, _productionEstimationValidator, _entityListErrorWrapper);
            ProductionPlan productionPlan = _productionPlanRepository.GetUnAddedEntityById(productionPlanId);
            ProductionRequirement productionRequirement = _productionRequirementRepository.GetUnAddedEntityById(productionRequirementId);
            // CheckTotalEstimationWithProductionPlanTime(inputDTOs,productionPlan);
            CheckTotalEstimationQuantityWithRequirementEntity(inputDTOs, productionRequirement);
            ValidateTimeInEstimationInputDTO(inputDTOs, productionPlan);
            foreach (ProductionEstimationInputDTO inputDTO in inputDTOs)
            {
                CheckSeriesExistInEstimation(inputDTO, inputDTOs.IndexOf(inputDTO) + 1, productionPlan);
                ProductionEstimation productionEstimation = _mapper.Map<ProductionEstimation>(inputDTO);
                productionEstimation.ProductionRequirementId = productionRequirementId;
                _productionEstimationRepository.Add(productionEstimation);
                if (productionPlan.Type.Equals(ProductionPlanType.Batch))
                {
                    await _productionSeriesService.AddList(inputDTO.ProductionSeries, productionEstimation.Id);
                }
            }
        }

        private void CheckSeriesExistInEstimation(ProductionEstimationInputDTO inputDTO, int entittyOrder, ProductionPlan productionPlan)
        {
            List<FormError> errors = new List<FormError>();
            if (productionPlan.Type.Equals(ProductionPlanType.Batch)
                && (inputDTO.ProductionSeries == null || inputDTO.ProductionSeries.Count <= 0))
            {
                errors.Add(new FormError
                {
                    EntityOrder = entittyOrder,
                    ErrorMessage = $"Production Series List is required when Production Plan Type is Batch",
                    Property = "ProductionSeries"
                });
            }
            else if (!productionPlan.Type.Equals(ProductionPlanType.Batch)
                    && inputDTO.ProductionSeries.Count > 0)
            {
                errors.Add(new FormError
                {
                    EntityOrder = entittyOrder,
                    ErrorMessage = $"Production Series List must be empty when Production Plan Type is not Batch",
                    Property = "ProductionSeries"
                });
            }
            if (errors.Count > 0)
            {
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<ProductionEstimation>(errors, _entityListErrorWrapper);
            }
        }

        private void CheckTotalEstimationQuantityWithRequirementEntity(
            List<ProductionEstimationInputDTO> inputDTOs, ProductionRequirement productionRequirement)
        {
            List<FormError> errors = new List<FormError>();
            List<ProductionRequirement> unAddedProductionRequirementList =
                _productionRequirementRepository.GetUnAddedEntityList();
            int quantitySum = 0;
            foreach (ProductionEstimationInputDTO inputDTO in inputDTOs)
            {
                quantitySum += inputDTO.Quantity;
            }
            if (quantitySum > productionRequirement.Quantity)
            {
                errors.Add(new FormError
                {
                    EntityOrder = unAddedProductionRequirementList.IndexOf(productionRequirement) + 1,
                    ErrorMessage = $"Total Quantity of Production Estimation list is greater than Quantity of Production Requirement",
                    Property = "Quantity"
                });
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<ProductionRequirement>(errors, _entityListErrorWrapper);
            }
        }

        private void ValidateTimeInEstimationInputDTO
        (List<ProductionEstimationInputDTO> inputDTOs, ProductionPlan productionPlan)
        {
            ProductionPlanType productionPlanType = productionPlan.Type;
            List<FormError> errors = new List<FormError>();
            foreach (ProductionEstimationInputDTO inputDTO in inputDTOs)
            {
                int entityOrder = inputDTOs.IndexOf(inputDTO) + 1;
                //Production Plan Type is Year
                if (productionPlanType.Equals(ProductionPlanType.Year))
                {
                    if (!inputDTO.Quarter.IsNullOrEmpty())
                    {
                        errors.Add(new FormError
                        {
                            EntityOrder = entityOrder,
                            ErrorMessage = $"Quarter must null when Production Plan Type is {productionPlanType}",
                            Property = "Quarter"
                        });
                    }
                    if (inputDTO.Month.IsNullOrEmpty())
                    {
                        errors.Add(new FormError
                        {
                            EntityOrder = entityOrder,
                            ErrorMessage = $"Month can not be null when Production Plan Type is {productionPlanType}",
                            Property = "Month"
                        });
                    }
                    else
                    {
                        if (!inputDTO.Month.IsNullOrEmpty() && !Enum.TryParse(typeof(Month), inputDTO.Month, true, out _))
                        {
                            errors.Add(new FormError
                            {
                                EntityOrder = entityOrder,
                                ErrorMessage = $"Invalid Month",
                                Property = "Month"
                            });
                        }
                    }
                    if (inputDTO.Batch.HasValue)
                    {
                        errors.Add(new FormError
                        {
                            EntityOrder = entityOrder,
                            ErrorMessage = $"Batch must null when Production Plan Type is {productionPlanType}",
                            Property = "Batch"
                        });
                    }
                    if (inputDTO.DayNumber.HasValue)
                    {
                        errors.Add(new FormError
                        {
                            EntityOrder = entityOrder,
                            ErrorMessage = $"DayNumber must null when Production Plan Type is {productionPlanType}",
                            Property = "DayNumber"
                        });
                    }
                }
                //Production Plan Type is Month
                else if (productionPlanType.Equals(ProductionPlanType.Month))
                {
                    if (!inputDTO.Quarter.IsNullOrEmpty())
                    {
                        errors.Add(new FormError
                        {
                            EntityOrder = entityOrder,
                            ErrorMessage = $"Quarter must null when Production Plan Type is {productionPlanType}",
                            Property = "Quarter"
                        });
                    }
                    if (!inputDTO.Month.IsNullOrEmpty())
                    {
                        errors.Add(new FormError
                        {
                            EntityOrder = entityOrder,
                            ErrorMessage = $"Month must null when Production Plan Type is {productionPlanType}",
                            Property = "Month"
                        });
                    }
                    if (!inputDTO.Batch.HasValue)
                    {
                        errors.Add(new FormError
                        {
                            EntityOrder = entityOrder,
                            ErrorMessage = $"Batch can not be null when Production Plan Type is {productionPlanType}",
                            Property = "Batch"
                        });
                    }
                    if (inputDTO.Batch.HasValue && inputDTO.Batch <= 0)
                    {
                        errors.Add(new FormError
                        {
                            EntityOrder = entityOrder,
                            ErrorMessage = "Batch must greater than 0",
                            Property = "Batch"
                        });
                    }
                    if (inputDTO.DayNumber.HasValue)
                    {
                        errors.Add(new FormError
                        {
                            EntityOrder = entityOrder,
                            ErrorMessage = $"DayNumber must null when Production Plan Type is {productionPlanType}",
                            Property = "DayNumber"
                        });
                    }
                }
                //Production Plan Type is Batch
                else if (productionPlanType.Equals(ProductionPlanType.Batch))
                {
                    if (!inputDTO.Quarter.IsNullOrEmpty())
                    {
                        errors.Add(new FormError
                        {
                            EntityOrder = entityOrder,
                            ErrorMessage = $"Quarter must null when Production Plan Type is {productionPlanType}",
                            Property = "Quarter"
                        });
                    }
                    if (!inputDTO.Month.IsNullOrEmpty())
                    {
                        errors.Add(new FormError
                        {
                            EntityOrder = entityOrder,
                            ErrorMessage = $"Month must null when Production Plan Type is {productionPlanType}",
                            Property = "Month"
                        });
                    }
                    if (inputDTO.Batch.HasValue)
                    {
                        errors.Add(new FormError
                        {
                            EntityOrder = entityOrder,
                            ErrorMessage = $"Batch must null when Production Plan Type is {productionPlanType}",
                            Property = "Batch"
                        });
                    }
                    if (!inputDTO.DayNumber.HasValue)
                    {
                        errors.Add(new FormError
                        {
                            EntityOrder = entityOrder,
                            ErrorMessage = $"DayNumber can not be null when Production Plan Type is {productionPlanType}",
                            Property = "DayNumber"
                        });
                    }
                    else
                    {
                        //lát khi viết tới add production plan child thì chỉnh tiếp chỗ này 
                        if (inputDTO.DayNumber <= 0)
                        {
                            errors.Add(new FormError
                            {
                                EntityOrder = entityOrder,
                                ErrorMessage = "DayNumber must greater than 0",
                                Property = "DayNumber"
                            });
                        }
                    }
                }
            }

            if (errors.Count == 0)
            {
                //Check duplication time in estimation
                //TH Production Plan Type Year
                if (productionPlanType.Equals(ProductionPlanType.Year))
                {
                    ServiceUtils.CheckFieldDuplicatedInInputDTOList<ProductionEstimationInputDTO, ProductionEstimation>(inputDTOs, "Month", _entityListErrorWrapper);
                    foreach (ProductionEstimationInputDTO inputDTO in inputDTOs)
                    {
                        Enum.TryParse(inputDTO.Month, true, out int parsedResult);
                        if (parsedResult < productionPlan.ExpectedStartingDate.Month
                            || parsedResult > productionPlan.DueDate.Month)
                        {
                            errors.Add(new FormError
                            {
                                EntityOrder = inputDTOs.IndexOf(inputDTO),
                                ErrorMessage = $"Production Estimation list with Month : {inputDTO.Month} not in Production Plan duration",
                                Property = "Month"
                            });
                        }
                    }
                }
                else if (productionPlanType.Equals(ProductionPlanType.Month))
                {

                }
                else
                {

                }
            }
            if (errors.Count > 0)
            {
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<ProductionEstimation>(errors, _entityListErrorWrapper);
            }
        }
    }
}
