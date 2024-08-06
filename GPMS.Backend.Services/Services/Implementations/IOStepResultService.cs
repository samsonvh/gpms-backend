using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Results;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class IOStepResultService : IIOStepResultService
    {
        private readonly IGenericRepository<ProductionProcessStepIOResult> _IOResultRepository;
        private readonly IMapper _mapper;

        public IOStepResultService(IGenericRepository<ProductionProcessStepIOResult> IOResultRepository, IMapper mapper)
        {
            _IOResultRepository = IOResultRepository;
            _mapper = mapper;   
        }
        public async Task<DefaultPageResponseListingDTO<IOResultListingDTO>> GetAllIOResultByStepResult(Guid stepResultId, IOResultFilterModel resultFilterModel)
        {
            var query = _IOResultRepository.GetAll().Where(ioResult => ioResult.StepResultId == stepResultId);
            query = query.SortBy<ProductionProcessStepIOResult>(resultFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery(resultFilterModel);
            var data = await query.ProjectTo<IOResultListingDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return new DefaultPageResponseListingDTO<IOResultListingDTO>
            {
                Data = data,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = resultFilterModel.Pagination.PageIndex,
                    PageSize = resultFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }

    }
}
