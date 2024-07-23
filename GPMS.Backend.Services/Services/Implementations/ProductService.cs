using System.Net;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Models.Warehouses;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<QualityStandard> _qualityStandardRepository;
        private readonly IValidator<ProductInputDTO> _productValidator;
        private readonly IValidator<ProductDefinitionInputDTO> _productDefinitionValidator;
        private readonly ICategoryService _categoryService;
        private readonly ISemiFinishedProductService _semiFinishedProductService;
        private readonly IMaterialService _materialService;
        private readonly ISpecificationService _specificationService;
        private readonly IProcessService _processService;
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly StepIOInputDTOWrapper _stepIOInputDTOWrapper;
        private readonly QualityStandardImagesTempWrapper _qualityStandardImagesTempWrapper;
        public ProductService(
        IGenericRepository<Product> productRepository,
        IGenericRepository<QualityStandard> qualityStandardRepository,
        IValidator<ProductInputDTO> productValidator,
        IValidator<ProductDefinitionInputDTO> productDefinitionValidator,
        ICategoryService categoryService,
        ISemiFinishedProductService semiFinishedProductService,
        IMaterialService materialService,
        ISpecificationService specificationService,
        IProcessService processService,
        IFirebaseStorageService firebaseStorageService,
        IMapper mapper,
        EntityListErrorWrapper entityListErrorWrapper,
        StepIOInputDTOWrapper stepIOInputDTOWrapper,
        QualityStandardImagesTempWrapper qualityStandardImagesTempWrapper
        )
        {
            _productRepository = productRepository;
            _qualityStandardRepository = qualityStandardRepository;
            _productValidator = productValidator;
            _productDefinitionValidator = productDefinitionValidator;
            _categoryService = categoryService;
            _semiFinishedProductService = semiFinishedProductService;
            _materialService = materialService;
            _specificationService = specificationService;
            _processService = processService;
            _firebaseStorageService = firebaseStorageService;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
            _stepIOInputDTOWrapper = stepIOInputDTOWrapper;
            _qualityStandardImagesTempWrapper = qualityStandardImagesTempWrapper;
        }

        public async Task<CreateUpdateResponseDTO<Product>> Add(ProductInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task AddList(List<ProductInputDTO> inputDTOs, Guid? parentId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDTO> Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductListingDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CreateUpdateResponseDTO<Product>> Update(ProductInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<ProductInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }

        private async Task<Guid> HandleAddCategory(string category)
        {
            CategoryDTO existedCategoryDTO = await _categoryService.DetailsByName(category);
            if (existedCategoryDTO == null)
            {
                CreateUpdateResponseDTO<Category> categoryCreateResponse = await _categoryService.Add(new CategoryInputDTO
                {
                    Name = category
                });
                return categoryCreateResponse.Id;
            }
            else return existedCategoryDTO.Id;
        }
        private Product HandleAddProduct(ProductDefinitionInputDTO inputDTO, Guid categoryId, CurrentLoginUserDTO currentLoginUserDTO)
        {
            Product product = _mapper.Map<Product>(inputDTO);
            product.CategoryId = categoryId;
            product.CreatorId = currentLoginUserDTO.StaffId;
            product.Status = ProductStatus.Pending;
            _productRepository.Add(product);
            return product;
        }

        public async Task<CreateUpdateResponseDTO<Product>> Add(ProductInputDTO inputDTO, CurrentLoginUserDTO currentLoginUserDTO)
        {
            List<ProductDefinitionInputDTO> definitionInputDTOs = new List<ProductDefinitionInputDTO>();
            ServiceUtils.ValidateInputDTO<ProductInputDTO, Product>
                (inputDTO, _productValidator, _entityListErrorWrapper);
            ServiceUtils.ValidateInputDTO<ProductDefinitionInputDTO, Product>
                (inputDTO.Definition, _productDefinitionValidator, _entityListErrorWrapper);
            definitionInputDTOs.Add(inputDTO.Definition);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<ProductDefinitionInputDTO, Product>
                (definitionInputDTOs, "Code", _entityListErrorWrapper);
            await ServiceUtils.CheckFieldDuplicatedWithInputDTOAndDatabase<ProductDefinitionInputDTO, Product>
                (inputDTO.Definition, _productRepository, "Code", "Code", _entityListErrorWrapper);
            Guid categoryId = await HandleAddCategory(inputDTO.Definition.Category);
            Product product = HandleAddProduct(inputDTO.Definition, categoryId, currentLoginUserDTO);
            //add semifinish product list
            List<CreateUpdateResponseDTO<SemiFinishedProduct>> semiFinishedProductCodeList =
            await _semiFinishedProductService.AddList(inputDTO.Definition.SemiFinishedProducts, product.Id);
            //add material list
            List<CreateUpdateResponseDTO<Material>> materialCodeList =
                await _materialService.AddList(inputDTO.Definition.Materials);
            //add specification list
            await _specificationService.AddList(inputDTO.Specifications, product.Id,
                materialCodeList, inputDTO.Definition.Sizes, inputDTO.Definition.Colors);
            //add process list
            await _processService.AddList(inputDTO.Processes, product.Id, materialCodeList, semiFinishedProductCodeList);
            ServiceUtils.CheckForeignEntityCodeListContainsAllForeignEntityCodeInInputDTOList
                <StepIOInputDTO, ProductionProcessStepIO, Material>
                (_stepIOInputDTOWrapper.StepIOInputDTOList.Where(stepIOInputDTO => !stepIOInputDTO.MaterialCode.IsNullOrEmpty()).ToList(),
                materialCodeList, "MaterialCode", "Code", _entityListErrorWrapper);
            ServiceUtils.CheckForeignEntityCodeListContainsAllForeignEntityCodeInInputDTOList
                <StepIOInputDTO, ProductionProcessStepIO, SemiFinishedProduct>
                (_stepIOInputDTOWrapper.StepIOInputDTOList.Where(stepIOInputDTO => !stepIOInputDTO.SemiFinishedProductCode.IsNullOrEmpty()).ToList(),
                semiFinishedProductCodeList, "SemiFinishedProductCode", "Code", _entityListErrorWrapper);
            if (_entityListErrorWrapper.EntityListErrors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Define Product Invalid", _entityListErrorWrapper);
            }
            await HandleUploadProductImage(inputDTO);
            await HandleUploadQualityStandardImage();
            await _productRepository.Save();
            return new CreateUpdateResponseDTO<Product>
            {
                Id = product.Id,
                Code = product.Code
            };
        }

        private async Task HandleUploadProductImage(ProductInputDTO inputDTO)
        {
            //upload product image
            string fileURL = "";
            Product unAddedProduct = _productRepository.GetUnAddedEntity();
            foreach (IFormFile file in inputDTO.Definition.Images)
            {
                if (file != null)
                {
                    string filePath = $"{typeof(Product).Name}/{unAddedProduct.Id}/Images/{file.FileName}";
                    string url = await _firebaseStorageService.UploadFile(filePath, file);
                    fileURL += url + ";";
                }
            }
            fileURL.Remove(fileURL.Length - 1);
            unAddedProduct.ImageURLs = fileURL;
        }
        private async Task HandleUploadQualityStandardImage()
        {
            //upload product image
            string fileURL = "";
            Product unAddedProduct = _productRepository.GetUnAddedEntity();
            List<QualityStandard> unAddedQualityStandardList = _qualityStandardRepository.GetUnAddedEntityList();
            foreach (QualityStandardImagesTemp qualityStandardImagesTemp in _qualityStandardImagesTempWrapper.QualityStandardImagesTemps)
            {
                QualityStandard qualityStandardImageAdd = unAddedQualityStandardList
                .Where(qualityStandard => qualityStandard.Id.Equals(qualityStandardImagesTemp.QualityStandardId))
                .FirstOrDefault();
                if (qualityStandardImageAdd != null)
                {
                    foreach (IFormFile file in qualityStandardImagesTemp.Files)
                    {
                        string filePath =
                        $"{typeof(Product).Name}/{unAddedProduct.Id}/{typeof(ProductSpecification).Name}/" +
                        $"{qualityStandardImageAdd.ProductSpecificationId}/{typeof(QualityStandard).Name}/" +
                        $"{qualityStandardImageAdd.Id}/Images/{file.FileName}";
                        string url = await _firebaseStorageService.UploadFile(filePath, file);
                        fileURL += url + ";";
                    }
                    fileURL.Remove(fileURL.Length - 1);
                    qualityStandardImageAdd.ImageURL = fileURL;
                    fileURL = "";
                }
            }
        }
    }
}