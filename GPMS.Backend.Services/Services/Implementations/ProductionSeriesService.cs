using AutoMapper;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
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

        public ProductionSeriesService(IGenericRepository<ProductionSeries> productionSeriesRepository, IMapper mapper)
        {
            _productionSeriesRepository = productionSeriesRepository;
            _mapper = mapper;
        }

        public async Task<List<CreateUpdateResponseDTO<ProductionSeries>>> AddList(List<ProductionSeriesInputDTO> inputDTOs, Guid productionEstimationId)
        {
            List<CreateUpdateResponseDTO<ProductionSeries>> responses = new List<CreateUpdateResponseDTO<ProductionSeries>>();
            foreach (ProductionSeriesInputDTO productionSeriesInputDTO in inputDTOs)
            {
                ProductionSeries productionSeries = _mapper.Map<ProductionSeries>(productionSeriesInputDTO);
                productionSeries.ProductionEstimationId = productionEstimationId;
                _productionSeriesRepository.Add(productionSeries);
                responses.Add(new CreateUpdateResponseDTO<ProductionSeries>
                {
                    Id = productionSeries.Id
                });
            }
            await _productionSeriesRepository.Save();
            return responses;
        }
    }
}
