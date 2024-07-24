using AutoMapper;
using FluentValidation;
using GPMS.Backend.Data.Enums.Statuses.Requests;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.InputDTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Requests;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class WarehouseRequestService : IWarehouseRequestService
    {
        private readonly IGenericRepository<WarehouseRequest> _warehouseRequestRepository;
        private readonly IGenericRepository<ProductionRequirement> _productionRequirementRepository;
        private readonly IMapper _mapper;
        private readonly IWarehouseRequestRequirementService _warehouseRequestRequirementService;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly IValidator<WarehouseRequestInputDTO> _warehouseRequestValidator;

        public WarehouseRequestService(IGenericRepository<WarehouseRequest> warehouseRequestRepository,
                                        IGenericRepository<ProductionRequirement> productionRequirementRepository,
                                        IMapper mapper,
                                        IWarehouseRequestRequirementService warehouseRequestRequirementService,
                                        EntityListErrorWrapper entityListErrorWrapper,
                                        IValidator<WarehouseRequestInputDTO> warehouseRequestValidator)
        {
            _warehouseRequestRepository = warehouseRequestRepository;
            _productionRequirementRepository = productionRequirementRepository;
            _mapper = mapper;
            _warehouseRequestRequirementService = warehouseRequestRequirementService;
            _entityListErrorWrapper = entityListErrorWrapper;
            _warehouseRequestValidator = warehouseRequestValidator;
        }

        public async Task<CreateUpdateResponseDTO<WarehouseRequest>> Add(WarehouseRequestInputDTO inputDTO)
        {
            ServiceUtils.ValidateInputDTO<WarehouseRequestInputDTO, WarehouseRequest>
                     (inputDTO, _warehouseRequestValidator, _entityListErrorWrapper);

            var warehouseRequest = _mapper.Map<WarehouseRequest>(inputDTO);
            warehouseRequest.CreatorId = inputDTO.CreatorId;
            warehouseRequest.Status = WarehouseRequestStatus.Pending;

            _warehouseRequestRepository.Add(warehouseRequest);

            int totalQuantity = 0;

            var warehouseRequestRequirementResponses = await _warehouseRequestRequirementService.AddListWarehouseRequestRequirement(inputDTO.WarehouseRequestRequirementInputDTOs, warehouseRequest.Id);

            totalQuantity = inputDTO.WarehouseRequestRequirementInputDTOs.Sum(req => req.Quantity);

            warehouseRequest.Quantity = totalQuantity;
            await _warehouseRequestRepository.Save();
            return _mapper.Map<CreateUpdateResponseDTO<WarehouseRequest>>(warehouseRequest);
        }

    }
}
