using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Utils;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IGenericRepository<Measurement> _measurementRepository;
        private readonly IValidator<MeasurementInputDTO> _measurementValidator;
        private readonly IMapper _mapper;

        public MeasurementService(
            IGenericRepository<Measurement> measurementRepository,
            IValidator<MeasurementInputDTO> measurementValidator,
            IMapper mapper
            )
        {
            _measurementRepository = measurementRepository;
            _measurementValidator = measurementValidator;
            _mapper = mapper;
        }

        public Task<CreateUpdateResponseDTO<Measurement>> Add(MeasurementInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<MeasurementInputDTO> inputDTOs, Guid? specificationId = null)
        {
            ServiceUtils.ValidateInputDTOList<MeasurementInputDTO,Measurement>(inputDTOs,_measurementValidator);
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

        public Task<CreateUpdateResponseDTO<Measurement>> Update(MeasurementInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<MeasurementInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }
    }
}