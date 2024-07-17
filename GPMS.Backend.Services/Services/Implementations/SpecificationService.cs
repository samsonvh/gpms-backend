using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class SpecificationService : ISpecificationService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductSpecification> _specificationRepository;
        private readonly IQualityStandardService _qualityStandardService;
        private readonly IMeasurementService _measurementService;
        private readonly IBillOfMaterialService _billOfMaterialService;
        private readonly IValidator<SpecificationInputDTO> _specificationValidator;
        private readonly IMapper _mapper;

        public SpecificationService(
            IGenericRepository<Product> productRepository,
            IGenericRepository<ProductSpecification> specificationRepository,
            IQualityStandardService qualityStandardService,
            IMeasurementService measurementService,
            IBillOfMaterialService billOfMaterialService,
            IValidator<SpecificationInputDTO> specificationValidator,
            IMapper mapper
            )
        {
            _productRepository = productRepository;
            _specificationRepository = specificationRepository;
            _qualityStandardService = qualityStandardService;
            _measurementService = measurementService;
            _billOfMaterialService = billOfMaterialService;
            _specificationValidator = specificationValidator;
            _mapper = mapper;
        }

        public Task<CreateUpdateResponseDTO<ProductSpecification>> Add(SpecificationInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<SpecificationInputDTO> inputDTOs, Guid? productId)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<SpecificationInputDTO> inputDTOs, Guid productId,
        List<CreateUpdateResponseDTO<Material>> materialCodeList, string sizes, string colors)
        {
            ValidateSpecificationInputDTOList(inputDTOs);
            ValidateSizeAndColorInSpecification(sizes,colors,inputDTOs);
            foreach (SpecificationInputDTO specificationInputDTO in inputDTOs)
            {
                ProductSpecification productSpecification = _mapper.Map<ProductSpecification>(specificationInputDTO);
                productSpecification.ProductId = productId;
                productSpecification.InventoryQuantity = 0;
                _specificationRepository.Add(productSpecification);
                await _measurementService.AddList(specificationInputDTO.Measurements, productSpecification.Id);
                await _billOfMaterialService.AddList(specificationInputDTO.BOMs, productSpecification.Id,materialCodeList);
                await _qualityStandardService.AddList(specificationInputDTO.QualityStandards, productSpecification.Id, materialCodeList);
            }
        }

        public Task<SpecificationDTO> Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<SpecificationListingDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CreateUpdateResponseDTO<ProductSpecification>> Update(SpecificationInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<SpecificationInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }
        private void ValidateSpecificationInputDTOList(List<SpecificationInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (SpecificationInputDTO inputDTO in inputDTOs)
            {
                FluentValidation.Results.ValidationResult validationResult = _specificationValidator.Validate(inputDTO);
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
                throw new APIException((int)HttpStatusCode.BadRequest, "Specification list invalid", errors);
            }
        }
        private void ValidateSizeAndColorInSpecification(string productSizes,
        string productColors, List<SpecificationInputDTO> specificationInputDTOs)
        {
            var sizes = productSizes.Split(",");
            var colors = productColors.Split(",");
            List<FormError> errors = new List<FormError>();
            foreach (SpecificationInputDTO specificationInputDTO in specificationInputDTOs)
            {
                if (!sizes.Contains(specificationInputDTO.Size))
                {
                    errors.Add(new FormError
                    {
                        Property = typeof(ProductSpecification).GetProperty("Size").Name,
                        ErrorMessage = $"Size: {specificationInputDTO.Size} of specification is not contained in sizes of product"
                    });
                }
                if (!colors.Contains(specificationInputDTO.Color))
                {
                    errors.Add(new FormError
                    {
                        Property = typeof(ProductSpecification).GetProperty("Color").Name,
                        ErrorMessage = $"Color: {specificationInputDTO.Color} of specification is not contained in colors of product"
                    });
                }
            }
            if (errors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Size/Color in specification invalid", errors);
            }
        }
    }
}