using AutoMapper;
using GPMS.Backend.Data.Enums.Statuses.Requests;
using GPMS.Backend.Data.Models.ProductionPlans;
using GPMS.Backend.Data.Models.Requests;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Requests;
using GPMS.Backend.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class InspectionRequestService : IInspectionRequestService
    {
        private readonly IGenericRepository<InspectionRequest> _inspectionRequestRepository;
        private readonly IGenericRepository<Staff> _staffRepository;
        private readonly IMapper _mapper;
        private readonly CurrentLoginUserDTO _currentLoginUser;
        private readonly IGenericRepository<ProductionSeries> _productionSeriesRepoitory;

        public InspectionRequestService(IGenericRepository<InspectionRequest> inspectionRequestRepository,
                                                       IGenericRepository<Staff> staffRepository,
                                                       IMapper mapper,
                                                       CurrentLoginUserDTO currentLoginUserDTO,
                                                       IGenericRepository<ProductionSeries> productionSeriesRepository)
        {
            _inspectionRequestRepository = inspectionRequestRepository;
            _staffRepository = staffRepository;
            _mapper = mapper;
            _currentLoginUser = currentLoginUserDTO;
            _productionSeriesRepoitory = productionSeriesRepository;
        }

        public async Task<InspectionRequestDTO> Add(InspectionRequestInputDTO inputDTO)
        {
            var productionSeries = await _productionSeriesRepoitory
                .Search(productionSeries => productionSeries.Id == inputDTO.ProductionSeriesId)
                .FirstOrDefaultAsync();

            if (productionSeries == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Production Series not found");
            }

            var inspectionRequest = _mapper.Map<InspectionRequest>(inputDTO);
            inspectionRequest.CreatorId = _currentLoginUser.Id;
            inspectionRequest.ProductionSeriesId = productionSeries.Id;
            inspectionRequest.Status = InspectionRequestStatus.Pending;

            var creator = await _staffRepository
                .Search(creator => creator.Id == _currentLoginUser.Id)
                .FirstOrDefaultAsync();

            if (creator == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Creator Staff not found");
            }

            _inspectionRequestRepository.Add(inspectionRequest);
            await _inspectionRequestRepository.Save();

            return _mapper.Map<InspectionRequestDTO>(inspectionRequest);
        }

        public async Task<InspectionRequestDTO> Details(Guid id)
        {
            var inspectionRequest = _inspectionRequestRepository
                .Search(inspectionRequest => inspectionRequest.Id == id)
                .Include(inspectionRequest => inspectionRequest.Creator)
                .Include(inspectionRequest => inspectionRequest.Reviewer)
                .FirstOrDefaultAsync();

            if (inspectionRequest == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Inspection Request not found");
            }

            return _mapper.Map<InspectionRequestDTO>(inspectionRequest);
        }
    }
}
