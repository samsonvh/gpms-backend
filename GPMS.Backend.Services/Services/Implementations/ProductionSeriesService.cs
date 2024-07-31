using AutoMapper;
using FluentValidation;
using GPMS.Backend.Data.Enums.Statuses.ProductionPlans;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Utils;
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
    }
}
