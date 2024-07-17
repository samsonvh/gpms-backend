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
            ValidateMeasurementInputDTOList(inputDTOs);
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

        public Task<MeasurementListingDTO> GetAll()
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
        private void ValidateMeasurementInputDTOList(List<MeasurementInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (MeasurementInputDTO inputDTO in inputDTOs)
            {
                FluentValidation.Results.ValidationResult validationResult = _measurementValidator.Validate(inputDTO);
                if (!validationResult.IsValid)
                {
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        errors.Add(new FormError
                        {
                            ErrorMessage = validationFailure.ErrorMessage,
                            Property = validationFailure.PropertyName
                        });
                    }
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Measurement list invalid", errors);
            }
        }
    }
}