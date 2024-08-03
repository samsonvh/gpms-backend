using AutoMapper;
using FluentValidation;
using GPMS.Backend.Data.Enums.Others;
using GPMS.Backend.Data.Enums.Statuses.Requests;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Enums.Types;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Models.Warehouses;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
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
using System.Net;
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
        private readonly IGenericRepository<WarehouseTicket> _warehouseTicketRepository;
        private readonly IGenericRepository<ProductSpecification> _productSpecificationRepository;
        private readonly IGenericRepository<Staff> _staffRepository;
        private readonly CurrentLoginUserDTO _currentLoginUser;
        public WarehouseRequestService(IGenericRepository<WarehouseRequest> warehouseRequestRepository,
                                        IGenericRepository<ProductionRequirement> productionRequirementRepository,
                                        IMapper mapper,
                                        IWarehouseRequestRequirementService warehouseRequestRequirementService,
                                        EntityListErrorWrapper entityListErrorWrapper,
                                        IValidator<WarehouseRequestInputDTO> warehouseRequestValidator,
                                        IGenericRepository<WarehouseTicket> warehouseTicketRepository,
                                        IGenericRepository<ProductSpecification> productSpecificationRepository,
                                        IGenericRepository<Staff> staffRepository,
                                        CurrentLoginUserDTO currentLoginUser)
        {
            _warehouseRequestRepository = warehouseRequestRepository;
            _productionRequirementRepository = productionRequirementRepository;
            _mapper = mapper;
            _warehouseRequestRequirementService = warehouseRequestRequirementService;
            _entityListErrorWrapper = entityListErrorWrapper;
            _warehouseRequestValidator = warehouseRequestValidator;
            _warehouseTicketRepository = warehouseTicketRepository;
            _productSpecificationRepository = productSpecificationRepository;
            _staffRepository = staffRepository;
            _currentLoginUser = currentLoginUser;
        }

        public async Task<CreateUpdateResponseDTO<WarehouseRequest>> Add(WarehouseRequestInputDTO inputDTO)
        {
            ServiceUtils.ValidateInputDTO<WarehouseRequestInputDTO, WarehouseRequest>
                     (inputDTO, _warehouseRequestValidator, _entityListErrorWrapper);

            var warehouseRequest = _mapper.Map<WarehouseRequest>(inputDTO);
            warehouseRequest.CreatorId = _currentLoginUser.Id;
            warehouseRequest.Status = WarehouseRequestStatus.Pending;

            _warehouseRequestRepository.Add(warehouseRequest);

            int totalQuantity = 0;

            var warehouseRequestRequirementResponses = await _warehouseRequestRequirementService.AddListWarehouseRequestRequirement(inputDTO.WarehouseRequestRequirements, warehouseRequest.Id);

            totalQuantity = inputDTO.WarehouseRequestRequirements.Sum(req => req.Quantity);

            warehouseRequest.Quantity = totalQuantity;
            await _warehouseRequestRepository.Save();
            return _mapper.Map<CreateUpdateResponseDTO<WarehouseRequest>>(warehouseRequest);
        }

        public async Task<ChangeStatusResponseDTO<WarehouseRequest, WarehouseRequestStatus>> ChangeStatus(Guid id, ChangeStatusInputDTO inputDTO)
        {
            var staff = await _staffRepository
                .Search(staff => staff.Id == _currentLoginUser.Id)
                .Include(staff => staff.Department)
                .FirstOrDefaultAsync(); 

            if (staff == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Staff not found.");

            }

            if(staff.Position.Equals(StaffPosition.Admin) || 
                staff.Position.Equals(StaffPosition.FactoryDirector) || staff.Position.Equals(StaffPosition.Staff))
            {
                throw new APIException((int)HttpStatusCode.Forbidden, "Admmin/Factory Director/Staff can change status of warehouse request.");
            }

            if(staff.Position.Equals(StaffPosition.Manager))
            {
                if(staff.Department.Name != "Warehouse Department")
                {
                    throw new APIException((int)HttpStatusCode.Forbidden, "Only warehouse manager can change warehouse request.");
                }
            }

            var warehouseRequest = await GetWarehouseRequestWithRequirements(id);

            ValidateWarehouseRequest(warehouseRequest, inputDTO.Status);

            //update status
            warehouseRequest.Status = Enum.Parse<WarehouseRequestStatus>(inputDTO.Status, true);

            if (warehouseRequest.Status == WarehouseRequestStatus.Declined)
            {
                if (string.IsNullOrWhiteSpace(inputDTO.Description))
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Description is required when declining the warehouse request.");
                }
            }

            if (warehouseRequest.Status == WarehouseRequestStatus.Approved)
            {
                await ApprovedWarehouseRequest(warehouseRequest);
            }

            warehouseRequest.ReviewerId = _currentLoginUser.Id;

            await _warehouseRequestRepository.Save();

            return _mapper.Map<ChangeStatusResponseDTO<WarehouseRequest, WarehouseRequestStatus>>(warehouseRequest);
        }

        private async Task<WarehouseRequest> GetWarehouseRequestWithRequirements(Guid id)
        {
            var warehouseRequest = await _warehouseRequestRepository
                .Search(warehouseRequest => warehouseRequest.Id == id)
                .Include(warehouseRequest => warehouseRequest.WarehouseRequestRequirements)
                .FirstOrDefaultAsync();

            if (warehouseRequest == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Warehouse request not found");
            }

            return warehouseRequest;
        }

        private WarehouseRequestStatus ValidateWarehouseRequest(WarehouseRequest warehouseRequest, string warehouseRequestStatus)
        {
            if (!Enum.TryParse(warehouseRequestStatus, true, out WarehouseRequestStatus parsedStatus))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Invalid status value provided.");
            }

            if(warehouseRequest.Status.Equals(WarehouseRequestStatus.Approved))
            {
                if(parsedStatus.Equals(WarehouseRequestStatus.Declined))
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Can not change status of warehouse request from Approved to Declined");
                }

                if(parsedStatus.Equals (WarehouseRequestStatus.Pending))
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Can not change status of warehouse request from Approved to Pending");
                }
            }

            if(warehouseRequest.Status.Equals(WarehouseRequestStatus.Declined))
            {
                if(parsedStatus.Equals(WarehouseRequestStatus.Approved))
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Can not change status of warehouse request from Declined to Approved");
                }
                
                if(parsedStatus.Equals(WarehouseRequestStatus.Pending))
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Can not change status of warehouse rerquest from Declined to Pending");
                }
            }

            return parsedStatus;
        }

        private async Task ApprovedWarehouseRequest(WarehouseRequest warehouseRequest)
        {
            var warehouseRequirement = warehouseRequest.WarehouseRequestRequirements.FirstOrDefault();
            if (warehouseRequirement == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Warehouse request has no requirements");
            }

            var productionRequirement = await _productionRequirementRepository
                .Search(pr => pr.Id == warehouseRequirement.ProductionRequirementId)
                .Include(pr => pr.ProductSpecification)
                .FirstOrDefaultAsync();

            if (productionRequirement == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, $"ProductionRequirement with Id: {warehouseRequirement.ProductionRequirementId} not found.");
            }

            var productSpecification = productionRequirement.ProductSpecification;
            if (productSpecification == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, $"ProductSpecification with Id: {productionRequirement.ProductSpecificationId} not found.");
            }

            var warehouseTicket = new WarehouseTicket
            {
                Name = warehouseRequest.Name,
                Quantity = warehouseRequest.Quantity,
                WarehouseRequestId = warehouseRequest.Id,
                Type = WarehouseTicketType.Import,
                ProductSpecificationId = productSpecification.Id,
                WarehouseId = productSpecification.WarehouseId
            };

            _warehouseTicketRepository.Add(warehouseTicket);
            await _warehouseTicketRepository.Save();
        }

        public async Task<WarehouseRequestDTO> Details(Guid id)
        {
            var staff = await _staffRepository
                .Search(staff => staff.Id == _currentLoginUser.Id)
                .Include(staff => staff.Department)
                .FirstOrDefaultAsync();

            if (staff == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Staff not found.");
            }

            if (!(staff.Position == StaffPosition.Manager && staff.Department.Name.Equals("Warehouse Department", StringComparison.OrdinalIgnoreCase)) &&
                !(staff.Position == StaffPosition.Manager && staff.Department.Name.Equals("Production Department", StringComparison.OrdinalIgnoreCase)))
            {
                throw new APIException((int)HttpStatusCode.Forbidden, "Only warehouse manager and production manager can access warehouse request details.");
            }

            var warehouseRequest = await _warehouseRequestRepository
                .Search(warehouseRequest => warehouseRequest.Id == id)
                .Include(warehouseRequest => warehouseRequest.WarehouseTicket)
                .FirstOrDefaultAsync();

            if (warehouseRequest == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Warehouse request not found");
            }

            warehouseRequest.ReviewerId = _currentLoginUser.Id;

            return _mapper.Map<WarehouseRequestDTO>(warehouseRequest);
        }
    }
}

