using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class ProcessService : IProcessService
    {
        private readonly IGenericRepository<ProductProductionProcess> _processRepository;
        private readonly IStepService _stepService;
        private readonly IValidator<ProcessInputDTO> _processValidator;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;

        public ProcessService(
            IGenericRepository<ProductProductionProcess> processRepository,
            IStepService stepService,
            IValidator<ProcessInputDTO> processValidator,
            IMapper mapper,
            EntityListErrorWrapper entityListErrorWrapper
            )
        {
            _processRepository = processRepository;
            _stepService = stepService;
            _processValidator = processValidator;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
        }

        public Task<CreateUpdateResponseDTO<ProductProductionProcess>> Add(ProcessInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<ProcessInputDTO> inputDTOs, Guid? productId = null)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<ProcessInputDTO> inputDTOs, Guid productId, 
        List<CreateUpdateResponseDTO<Material>> materialCodeList, 
        List<CreateUpdateResponseDTO<SemiFinishedProduct>> semiFinishedProductCodeList)
        {
            ServiceUtils.ValidateInputDTOList<ProcessInputDTO,ProductProductionProcess>
                (inputDTOs,_processValidator,_entityListErrorWrapper);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<ProcessInputDTO,ProductProductionProcess>
                (inputDTOs,"Code",_entityListErrorWrapper);
            await ServiceUtils.CheckFieldDuplicatedWithInputDTOListAndDatabase<ProcessInputDTO,ProductProductionProcess>
                (inputDTOs,_processRepository,"Code","Code",_entityListErrorWrapper);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<ProcessInputDTO,ProductProductionProcess>
                (inputDTOs,"OrderNumber",_entityListErrorWrapper);
            inputDTOs = inputDTOs.OrderBy(processInputDTO => processInputDTO.OrderNumber).ToList();
            foreach (ProcessInputDTO processInputDTO in inputDTOs)
            {
                ProductProductionProcess productProductionProcess = _mapper.Map<ProductProductionProcess>(processInputDTO);
                productProductionProcess.ProductId = productId;
                _processRepository.Add(productProductionProcess);
                await _stepService.AddList(processInputDTO.Steps, productProductionProcess.Id,
                materialCodeList,semiFinishedProductCodeList);
            }
        }

        public Task<ProcessDTO> Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponseListingDTO<ProcessListingDTO>> GetAll(ProcessFilterModel processFilterModel)
        {
            var query = _processRepository.GetAll();
            query = Filters(query, processFilterModel);
            query = query.SortBy<ProductProductionProcess>(processFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery(processFilterModel);
            var data = await query.ProjectTo<ProcessListingDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return new DefaultPageResponseListingDTO<ProcessListingDTO>
            {
                Data = data,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = processFilterModel.Pagination.PageIndex,
                    PageSize = processFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }

        private IQueryable<ProductProductionProcess> Filters(IQueryable<ProductProductionProcess> query, ProcessFilterModel processFilterModel)
        {
            if (!processFilterModel.Code.IsNullOrEmpty())
            {
                query = query.Where(process => process.Code.Contains(processFilterModel.Code));
            }
            if (!processFilterModel.Name.IsNullOrEmpty())
            {
                query = query.Where(process => process.Name.Contains(processFilterModel.Name));
            }
            return query;
        }

        public Task<CreateUpdateResponseDTO<ProductProductionProcess>> Update(ProcessInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<ProcessInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponseListingDTO<ProcessListingDTO>> GetAllProcessOfProduct(Guid productId, ProcessFilterModel processFilterModel)
        {
            IQueryable<ProductProductionProcess> query = _processRepository.GetAll();
            query = query.Where(process => process.ProductId == productId);
            query = Filters(query, processFilterModel);
            int totalItem = query.Count();
            query = query.SortBy<ProductProductionProcess>(processFilterModel);
            query = query.PagingEntityQuery(processFilterModel);
            var data = await query.ProjectTo<ProcessListingDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return new DefaultPageResponseListingDTO<ProcessListingDTO>
            {
                Data = data,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = processFilterModel.Pagination.PageIndex,
                    PageSize = processFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }
    }
}