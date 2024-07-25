using AutoMapper;
using FluentValidation;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.InputDTOs.ProductionPlan;
using GPMS.Backend.Services.DTOs.Product.InputDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class ProductionRequirementService : IProductionRequirementService
    {
        private readonly IGenericRepository<ProductionRequirement> _productionRequirementRepository;
        private readonly IGenericRepository<ProductSpecification> _productSpecificationRepository;
        private readonly IMapper _mapper;

        public ProductionRequirementService(IGenericRepository<ProductionRequirement> productionRequirementRepository,
                                            IGenericRepository<ProductSpecification> productSpecificationRepository,
                                            IMapper mapper)
        {
            _productionRequirementRepository = productionRequirementRepository;
            _productSpecificationRepository = productSpecificationRepository;
            _mapper = mapper;
        }

        public async Task<List<CreateUpdateResponseDTO<ProductionRequirement>>> AddList(List<ProductionRequirementInputDTO> inputDTOs, Guid productionPlanId)
        {
            /*ServiceUtils.ValidateInputDTOList<ProductionRequirementInputDTO, ProductionRequirement>
            (inputDTOs, _productionRequirementValidator);*/

            var responses = new List<CreateUpdateResponseDTO<ProductionRequirement>>();

            foreach (var productionRequirementInputDTO in inputDTOs)
            {
                await ValidateSpecification(productionRequirementInputDTO.ProductionSpecificationId);

                var productionRequirement = _mapper.Map<ProductionRequirement>(productionRequirementInputDTO);
                productionRequirement.ProductionPlanId = productionPlanId;
                productionRequirement.ProductSpecificationId = productionRequirementInputDTO.ProductionSpecificationId;

                _productionRequirementRepository.Add(productionRequirement);
                await _productionRequirementRepository.Save();

                responses.Add(new CreateUpdateResponseDTO<ProductionRequirement>
                {
                    Id = productionRequirement.Id
                });
            }

            return responses;
        }

        private async Task ValidateSpecification(Guid productSpecificationId)
        {
            var productionSpecification = await _productSpecificationRepository
                .Search(spec => spec.Id == productSpecificationId)
                .FirstOrDefaultAsync();

            if (productionSpecification == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, $"Specification not found with Id {productSpecificationId}");
            }
        }
    }
}
