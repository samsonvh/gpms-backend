using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.NetworkInformation;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Warehouses;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class SpecificationService : ISpecificationService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductSpecification> _specificationRepository;
        private readonly IFirebaseStorageService _firebaseStorageService;
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
            IFirebaseStorageService firebaseStorageService,
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
            _firebaseStorageService = firebaseStorageService;
            _qualityStandardService = qualityStandardService;
            _measurementService = measurementService;
            _billOfMaterialService = billOfMaterialService;
            _warehouseRepository = warehouseRepository;
            _specificationValidator = specificationValidator;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
        }


        public async Task AddList(List<SpecificationInputDTO> inputDTOs, Guid? productId)
        {
            throw new NotImplementedException();
        }
        #region Add List
        public async Task AddList(List<SpecificationInputDTO> inputDTOs, Guid productId, string sizes, string colors)
        {
            ServiceUtils.ValidateInputDTOList<SpecificationInputDTO, ProductSpecification>
                (inputDTOs, _specificationValidator, _entityListErrorWrapper);
            ValidateSizeAndColorInSpecification(sizes, colors, inputDTOs);
            Warehouse existedProductWarehouse =
                await _warehouseRepository.Search(warehouse => warehouse.Name.Equals("Product Warehouse"))
                                            .FirstOrDefaultAsync();
            foreach (SpecificationInputDTO inputDTO in inputDTOs)
            {
                ProductSpecification productSpecification = _mapper.Map<ProductSpecification>(inputDTO);
                productSpecification.ProductId = productId;
                productSpecification.WarehouseId = existedProductWarehouse.Id;
                productSpecification.InventoryQuantity = 0;
                _specificationRepository.Add(productSpecification);
                await _measurementService.AddList(inputDTO.Measurements, productSpecification.Id);
                await _billOfMaterialService.AddList(inputDTO.BOMs, productSpecification.Id);
                var materialIds = inputDTO.BOMs.Select(bom => bom.MaterialId).ToList();
                await _qualityStandardService.AddList(inputDTO.QualityStandards, productSpecification.Id, materialIds);
            }
        }

        private void ValidateSizeAndColorInSpecification(string productSizes,
            string productColors, List<SpecificationInputDTO> specificationInputDTOs)
        {
            var sizes = productSizes.Split(",", StringSplitOptions.TrimEntries).Select(s => s.ToUpper()).ToArray();
            var colors = productColors.Split(",", StringSplitOptions.TrimEntries).Select(s => s.ToUpper()).ToArray();
            List<FormError> errors = new List<FormError>();
            foreach (SpecificationInputDTO specificationInputDTO in specificationInputDTOs)
            {
                if (!sizes.Contains(specificationInputDTO.Size.Trim().ToUpper()))
                {
                    errors.Add(new FormError
                    {
                        Property = "Size",
                        ErrorMessage = $"Size: {specificationInputDTO.Size} of specification is not existed in sizes of product"
                    });
                }
                if (!colors.Contains(specificationInputDTO.Color.Trim().ToUpper()))
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
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<ProductSpecification>(errors, _entityListErrorWrapper);
            }
        }

        #endregion
        #region Get Detail By Id
        public async Task<SpecificationDTO> Details(Guid id)
        {
            var specificationExisted = await
            _specificationRepository.Search(spec => spec.Id.Equals(id))
            .Include(spec => spec.Measurements)
            .Include(spec => spec.BillOfMaterials)
            .Include(spec => spec.QualityStandards)
            .FirstOrDefaultAsync();
            if (specificationExisted == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Specification Not Found");
            }
            return _mapper.Map<SpecificationDTO>(specificationExisted);
        }
        #endregion
        #region  Get All
        public async Task<DefaultPageResponseListingDTO<SpecificationListingDTO>> GetAll(SpecificationFilterModel specificationFilter)
        {
            var query = _specificationRepository.GetAll();
            query = Filters(query, specificationFilter);
            query = query.SortBy<ProductSpecification>(specificationFilter);
            int totalItem = query.Count();
            query = query.PagingEntityQuery(specificationFilter);
            var data = await query.ProjectTo<SpecificationListingDTO>(_mapper.ConfigurationProvider)
                                    .ToListAsync();

            return new DefaultPageResponseListingDTO<SpecificationListingDTO>
            {
                Data = data,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = specificationFilter.Pagination.PageIndex,
                    PageSize = specificationFilter.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }

        private IQueryable<ProductSpecification> Filters(IQueryable<ProductSpecification> query, SpecificationFilterModel specificationFilter)
        {
            if (!specificationFilter.Size.IsNullOrEmpty())
            {
                query = query.Where(process => process.Size.Contains(specificationFilter.Size));
            }
            if (!specificationFilter.Color.IsNullOrEmpty())
            {
                query = query.Where(process => process.Color.Contains(specificationFilter.Color));
            }
            return query;
        }
        #endregion


        public Task UpdateList(List<SpecificationInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }


        public Task<SpecificationDTO> Add(SpecificationInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task<SpecificationDTO> Update(Guid id, SpecificationInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        #region Upload Image
        public async Task<SpecificationDTO> UploadImages(ImageSpecificationInputDTO inputDTO)
        {
            var existedSpecification = await _specificationRepository.Search(spec => spec.Id.Equals(inputDTO.SpecificationId))
                                                                .Include(spec => spec.Product)
                                                                .FirstOrDefaultAsync();
            if (existedSpecification == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Specification Not Found");
            }
            var updatedSpecification = await HandleUploadSpecificationImage(inputDTO, existedSpecification);
            return _mapper.Map<SpecificationDTO>(updatedSpecification);
        }
        private async Task<ProductSpecification> HandleUploadSpecificationImage
            (ImageSpecificationInputDTO inputDTO, ProductSpecification specification)
        {
            //upload specification image
            string fileURL = "";
            foreach (IFormFile file in inputDTO.Images)
            {
                if (file != null)
                {
                    string filePath = $"{typeof(Product).Name}/{specification.Product.Id}/{typeof(ProductSpecification).Name}/{specification.Id}/Images/{file.FileName}";
                    string url = await _firebaseStorageService.UploadFile(filePath, file);
                    fileURL += url + ";";
                }
            }
            fileURL = fileURL.Remove(fileURL.Length - 1);
            specification.ImageURLs = fileURL;
            _specificationRepository.Update(specification);
            await _specificationRepository.Save();
            return specification;
        }
        #endregion
        #region Get All By ProductId
        public async Task<DefaultPageResponseListingDTO<SpecificationListingDTO>> GetAllSpcificationByProductId(Guid productId, SpecificationFilterModel specificationFilterModel)
        {
            var query = _specificationRepository.GetAll().Where(query => query.ProductId == productId);
            query = Filters(query, specificationFilterModel);
            query = query.SortBy<ProductSpecification>(specificationFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery<ProductSpecification>(specificationFilterModel);
            var specifications = await query.ProjectTo<SpecificationListingDTO>(_mapper.ConfigurationProvider)
                                        .ToListAsync();
            return new DefaultPageResponseListingDTO<SpecificationListingDTO>
            {
                Data = specifications,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = specificationFilterModel.Pagination.PageIndex,
                    PageSize = specificationFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }
        #endregion
    }
}