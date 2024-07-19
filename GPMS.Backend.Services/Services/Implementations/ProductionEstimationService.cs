using AutoMapper;
using FluentValidation;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Utils;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class ProductionEstimationService : IProductionEstimationService
    {
        private readonly IGenericRepository<ProductionEstimation> _productionEstimationRepository;
        private readonly IMapper _mapper;

        public ProductionEstimationService(IMapper mapper,
                                           IGenericRepository<ProductionEstimation> productionEstimationRepository)
        {
            _mapper = mapper;
            _productionEstimationRepository = productionEstimationRepository;
        }

        public async Task<List<CreateUpdateResponseDTO<ProductionEstimation>>> AddList(List<ProductionEstimationInputDTO> inputDTOs, Guid productionRequirementId)
        {
            List<CreateUpdateResponseDTO<ProductionEstimation>> responses = new List<CreateUpdateResponseDTO<ProductionEstimation>>();
            foreach (var productionEstimationInputDTO in inputDTOs)
            {
                ProductionEstimation productionEstimation = _mapper.Map<ProductionEstimation>(productionEstimationInputDTO);
                productionEstimation.ProductionRequirementId = productionRequirementId;
                _productionEstimationRepository.Add(productionEstimation);
                responses.Add(new CreateUpdateResponseDTO<ProductionEstimation>
                {
                    Id = productionEstimation.Id
                });
            }

            await _productionEstimationRepository.Save();
            return responses;
        }
    }
}
