using AutoMapper;
using Azure;
using FluentValidation;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.Requests;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Utils;
using GPMS.Backend.Services.Utils.Validators;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class WarehouseRequestRequirementService : IWarehouseRequestRequirementService
    {
        private readonly IGenericRepository<WarehouseRequestRequirement> _warehouseRequestRequirementRepository;
        private readonly IValidator<WarehouseRequestRequirementInputDTO> _warehouseRequestRequirementValidator;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly WarehouseRequestRequirementInputDTOWrapper _warehouseRequestRequirementInputDTOWrapper;
        private readonly IGenericRepository<ProductionRequirement> _productionRequirementRepository;

        public WarehouseRequestRequirementService(IGenericRepository<WarehouseRequestRequirement> warehouseRequestRequirementRepository,
                                                    IValidator<WarehouseRequestRequirementInputDTO> warehouseRequestRequirementValidator,
                                                    IMapper mapper,
                                                    EntityListErrorWrapper entityListErrorWrapper,
                                                    WarehouseRequestRequirementInputDTOWrapper warehouseRequestRequirementInputDTOWrapper,
                                                    IGenericRepository<ProductionRequirement> productionRequirementRepository)
        {
            _warehouseRequestRequirementRepository = warehouseRequestRequirementRepository;
            _warehouseRequestRequirementValidator = warehouseRequestRequirementValidator;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
            _warehouseRequestRequirementInputDTOWrapper = warehouseRequestRequirementInputDTOWrapper;
            _productionRequirementRepository = productionRequirementRepository;
        }


        public async Task<List<CreateUpdateResponseDTO<WarehouseRequestRequirement>>> AddListWarehouseRequestRequirement(List<WarehouseRequestRequirementInputDTO> inputDTOs, Guid warehouseRequestId)
        {
            ServiceUtils.ValidateInputDTOList<WarehouseRequestRequirementInputDTO, WarehouseRequestRequirement>
                (inputDTOs, _warehouseRequestRequirementValidator, _entityListErrorWrapper);

            ValidateRequirements(inputDTOs);

            List<WarehouseRequestRequirement> warehouseRequestRequirements = new List<WarehouseRequestRequirement>();

            foreach (var inputDTO in inputDTOs)
            {
                var warehouseRequestRequirement = _mapper.Map<WarehouseRequestRequirement>(inputDTO);
                warehouseRequestRequirement.WarehouseRequestId = warehouseRequestId;
                _warehouseRequestRequirementRepository.Add(warehouseRequestRequirement);
                warehouseRequestRequirements.Add(warehouseRequestRequirement);
            }

            List<CreateUpdateResponseDTO<WarehouseRequestRequirement>> responses = new List<CreateUpdateResponseDTO<WarehouseRequestRequirement>>();
            foreach (var requirement in warehouseRequestRequirements)
            {
                responses.Add(new CreateUpdateResponseDTO<WarehouseRequestRequirement>
                {
                    Id = requirement.Id,
                });
            }
            if (_entityListErrorWrapper.EntityListErrors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, " Invalid", _entityListErrorWrapper);
            }
            return responses;
        }


        private void ValidateRequirements(List<WarehouseRequestRequirementInputDTO> inputDTOs)
        {
            var productionRequirementErrors = new List<FormError>();

            var warehouseRequestRequirementErrors = new List<FormError>();

            var alreadyReportedErrors = new HashSet<Guid>();

            var quantityTracker = new Dictionary<Guid, int>();

            foreach (var inputDTO in inputDTOs)
            {
                var productionRequirement = _productionRequirementRepository
                   .Details(inputDTO.ProducitonRequirementId);

                if (productionRequirement == null)
                {
                    productionRequirementErrors.Add(new FormError
                    {
                        Property = nameof(WarehouseRequestRequirementInputDTO.ProducitonRequirementId),
                        ErrorMessage = $"ProductionRequirement with Id: {inputDTO.ProducitonRequirementId} not found.",
                        EntityOrder = inputDTOs.IndexOf(inputDTO) + 1
                    });
                    alreadyReportedErrors.Add(inputDTO.ProducitonRequirementId);

                    continue;
                }

                // Update the total quantity for this ProducitonRequirementId
                if (!quantityTracker.ContainsKey(inputDTO.ProducitonRequirementId))
                {
                    quantityTracker[inputDTO.ProducitonRequirementId] = 0;
                }

                quantityTracker[inputDTO.ProducitonRequirementId] += inputDTO.Quantity;

                if (quantityTracker[inputDTO.ProducitonRequirementId] > productionRequirement.Quantity)
                {
                    if (!alreadyReportedErrors.Contains(inputDTO.ProducitonRequirementId))
                    {
                        warehouseRequestRequirementErrors.Add(new FormError
                        {
                            Property = nameof(WarehouseRequestRequirementInputDTO.Quantity),
                            ErrorMessage = $"Quantity for ProductionRequirement with Id: {inputDTO.ProducitonRequirementId} exceeds available quantity.",
                            EntityOrder = inputDTOs.IndexOf(inputDTO) + 1
                        });
                        alreadyReportedErrors.Add(inputDTO.ProducitonRequirementId);
                    }
                }
            }

            if (productionRequirementErrors.Count > 0)
            {
                _entityListErrorWrapper.EntityListErrors.Add(new EntityListError
                {
                    Entity = "ProductionRequirement",
                    Errors = productionRequirementErrors
                });
            }

            if (warehouseRequestRequirementErrors.Count > 0)
            {
                _entityListErrorWrapper.EntityListErrors.Add(new EntityListError
                {
                    Entity = "WarehouseRequestRequirement",
                    Errors = warehouseRequestRequirementErrors
                });
            }
        }

        /*private void ValidateRequirements(List<WarehouseRequestRequirementInputDTO> inputDTOs)
        {
            var productionRequirementErrors = new List<FormError>();
            var warehouseRequestRequirementErrors = new List<FormError>();

            var quantityTracker = new Dictionary<Guid, int>();

            // Check each inputDTO
            foreach (var inputDTO in inputDTOs)
            {
                var productionRequirement = _productionRequirementRepository
                    .Details(inputDTO.ProducitonRequirementId);

                // Check if ProductionRequirement exists
                if (productionRequirement == null)
                {
                    productionRequirementErrors.Add(new FormError
                    {
                        Property = nameof(WarehouseRequestRequirementInputDTO.ProducitonRequirementId),
                        ErrorMessage = $"ProductionRequirement with Id: {inputDTO.ProducitonRequirementId} not found.",
                        EntityOrder = inputDTOs.IndexOf(inputDTO) + 1
                    });
                }
                else
                {
                    // Update the quantity tracker
                    if (!quantityTracker.ContainsKey(inputDTO.ProducitonRequirementId))
                    {
                        quantityTracker[inputDTO.ProducitonRequirementId] = 0;
                    }
                    quantityTracker[inputDTO.ProducitonRequirementId] += inputDTO.Quantity;

                    // Check if the quantity exceeds the available quantity
                    if (quantityTracker[inputDTO.ProducitonRequirementId] > productionRequirement.Quantity)
                    {
                        warehouseRequestRequirementErrors.Add(new FormError
                        {
                            Property = nameof(WarehouseRequestRequirementInputDTO.Quantity),
                            ErrorMessage = $"Quantity for ProductionRequirement with Id: {inputDTO.ProducitonRequirementId} exceeds available quantity.",
                            EntityOrder = inputDTOs.IndexOf(inputDTO) + 1
                        });
                    }
                }
            }

            // Add errors to response
            if (productionRequirementErrors.Count > 0)
            {
                _entityListErrorWrapper.EntityListErrors.Add(new EntityListError
                {
                    Entity = "ProductionRequirement",
                    Errors = productionRequirementErrors
                });
            }

            if (warehouseRequestRequirementErrors.Count > 0)
            {
                _entityListErrorWrapper.EntityListErrors.Add(new EntityListError
                {
                    Entity = "WarehouseRequestRequirement",
                    Errors = warehouseRequestRequirementErrors
                });
            }
        }*/
    }
}
