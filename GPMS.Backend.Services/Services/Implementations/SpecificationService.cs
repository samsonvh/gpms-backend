using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Warehouses;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class SpecificationService : ISpecificationService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductSpecification> _specificationRepository;
        private readonly IQualityStandardService _qualityStandardService;
        private readonly IMeasurementService _measurementService;
        private readonly IBillOfMaterialService _billOfMaterialService;
        private readonly IGenericRepository<Warehouse> _warehouseRepository;
        private readonly IValidator<SpecificationInputDTO> _specificationValidator;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;

        public SpecificationService(
            IGenericRepository<Product> productRepository,
            IGenericRepository<ProductSpecification> specificationRepository,
            IQualityStandardService qualityStandardService,
            IMeasurementService measurementService,
            IBillOfMaterialService billOfMaterialService,
            IGenericRepository<Warehouse> warehouseRepository,
            IValidator<SpecificationInputDTO> specificationValidator,
            IMapper mapper,
            EntityListErrorWrapper entityListErrorWrapper
            )
        {
            _productRepository = productRepository;
            _specificationRepository = specificationRepository;
            _qualityStandardService = qualityStandardService;
            _measurementService = measurementService;
            _billOfMaterialService = billOfMaterialService;
            _warehouseRepository = warehouseRepository;
            _specificationValidator = specificationValidator;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
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
            ServiceUtils.ValidateInputDTOList<SpecificationInputDTO,ProductSpecification>
                (inputDTOs,_specificationValidator,_entityListErrorWrapper);
            ValidateSizeAndColorInSpecification(sizes,colors,inputDTOs);
            Warehouse existedProductWarehouse = await _warehouseRepository.Search(warehouse => warehouse.Name.Equals("Product Warehouse")).FirstOrDefaultAsync();
            foreach (SpecificationInputDTO specificationInputDTO in inputDTOs)
            {
                ProductSpecification productSpecification = _mapper.Map<ProductSpecification>(specificationInputDTO);
                productSpecification.ProductId = productId;
                productSpecification.WarehouseId = existedProductWarehouse.Id;
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

        public Task<List<SpecificationListingDTO>> GetAll()
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
        private void ValidateSizeAndColorInSpecification(string productSizes,
        string productColors, List<SpecificationInputDTO> specificationInputDTOs)
        {
            var sizes = productSizes.Split(",",StringSplitOptions.TrimEntries);
            var colors = productColors.Split(",",StringSplitOptions.TrimEntries);
            List<FormError> errors = new List<FormError>();
            foreach (SpecificationInputDTO specificationInputDTO in specificationInputDTOs)
            {
                if (!sizes.Contains(specificationInputDTO.Size.Trim()))
                {
                    errors.Add(new FormError
                    {
                        Property = "Size",
                        ErrorMessage = $"Size: {specificationInputDTO.Size} of specification is not existed in sizes of product"
                    });
                }
                if (!colors.Contains(specificationInputDTO.Color.Trim()))
                {
                    errors.Add(new FormError
                    {
                        Property = "Color",
                        ErrorMessage = $"Color: {specificationInputDTO.Color} of specification is not existed in colors of product"
                    });
                }
            }
            if (errors.Count > 0)
            {
                EntityListError entityListError = new EntityListError
                {
                    Entity = typeof(ProductSpecification).Name,
                    Errors = errors
                };
                _entityListErrorWrapper.EntityListErrors.Add(entityListError);
            }
        }
    }
}