using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Enums.Statuses.Staffs;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IGenericRepository<Measurement> _measurementRepository;
        private readonly IValidator<MeasurementInputDTO> _measurementValidator;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;

        public MeasurementService(
            IGenericRepository<Measurement> measurementRepository,
            IValidator<MeasurementInputDTO> measurementValidator,
            IMapper mapper,
            EntityListErrorWrapper entityListErrorWrapper
            )
        {
            _measurementRepository = measurementRepository;
            _measurementValidator = measurementValidator;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
        }



        public async Task AddList(List<MeasurementInputDTO> inputDTOs, Guid? specificationId = null)
        {
            ServiceUtils.ValidateInputDTOList<MeasurementInputDTO,Measurement>
                (inputDTOs,_measurementValidator,_entityListErrorWrapper);
            foreach (MeasurementInputDTO measurementInputDTO in inputDTOs)
            {
                Measurement measurement = _mapper.Map<Measurement>(measurementInputDTO);
                measurement.ProductSpecificationId = (Guid)specificationId;
                _measurementRepository.Add(measurement);
            }
        }

        public Task<MeasurementDTO> Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<MeasurementListingDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponseListingDTO<MeasurementListingDTO>> GetAll(MeasurementFilterModel filter)
        {
            var query = _measurementRepository.GetAll();
            query = Filters(query, filter);
            query = query.SortBy<Measurement>(filter);
            int totalItem = query.Count();
            query = query.PagingEntityQuery<Measurement>(filter);
            var measurements = await query.ProjectTo<MeasurementListingDTO>(_mapper.ConfigurationProvider)
                                        .ToListAsync();
            return new DefaultPageResponseListingDTO<MeasurementListingDTO>
            {
                Data = measurements,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = filter.Pagination.PageIndex,
                    PageSize = filter.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }

        private IQueryable<Measurement> Filters(IQueryable<Measurement> query, MeasurementFilterModel measurementFilterModel)
        {
            if (!measurementFilterModel.Name.IsNullOrEmpty())
            {
                query = query.Where(measurement => measurement.Name.Contains(measurementFilterModel.Name));
            }
            return query;
        }


        public Task UpdateList(List<MeasurementInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }

        public Task<MeasurementDTO> Add(MeasurementInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task<MeasurementDTO> Update(Guid id, MeasurementInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }
    }
}