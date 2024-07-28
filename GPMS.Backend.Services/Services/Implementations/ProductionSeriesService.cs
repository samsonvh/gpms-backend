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
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly IValidator<ProductionSeriesInputDTO> _productionSeriesValidator;

        public ProductionSeriesService(
            IGenericRepository<ProductionSeries> productionSeriesRepository, 
            IMapper mapper,
            EntityListErrorWrapper entityListErrorWrapper,
            IValidator<ProductionSeriesInputDTO> productionSeriesValidator)
        {
            _productionSeriesRepository = productionSeriesRepository;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
            _productionSeriesValidator = productionSeriesValidator;
        }

        public async Task AddList(List<ProductionSeriesInputDTO> inputDTOs, Guid productionEstimationId)
        {
            ServiceUtils.ValidateInputDTOList<ProductionSeriesInputDTO,ProductionSeries>
                (inputDTOs,_productionSeriesValidator,_entityListErrorWrapper);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<ProductionSeriesInputDTO,ProductionSeries>
                (inputDTOs,"Code",_entityListErrorWrapper);
            await ServiceUtils.CheckFieldDuplicatedWithInputDTOListAndDatabase<ProductionSeriesInputDTO,ProductionSeries>
                (inputDTOs,_productionSeriesRepository,"Code","Code",_entityListErrorWrapper);
            List<CreateUpdateResponseDTO<ProductionSeries>> responses = new List<CreateUpdateResponseDTO<ProductionSeries>>();
            foreach (ProductionSeriesInputDTO productionSeriesInputDTO in inputDTOs)
            {
                ProductionSeries productionSeries = _mapper.Map<ProductionSeries>(productionSeriesInputDTO);
                productionSeries.ProductionEstimationId = productionEstimationId;
                productionSeries.Status = ProductionSeriesStatus.Pending;
                _productionSeriesRepository.Add(productionSeries);
            }
        }
    }
}
