using System.Net;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Warehouses;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Process;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.Product.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Utils;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductionProcessStepIO> _stepIORepository;
        private readonly IGenericRepository<Material> _materialRepository;
        private readonly IValidator<ProductInputDTO> _productValidator;
        private readonly IValidator<ProductDefinitionInputDTO> _productDefinitionValidator;
        private readonly ICategoryService _categoryService;
        private readonly ISemiFinishedProductService _semiFinishedProductService;
        private readonly IMaterialService _materialService;
        private readonly ISpecificationService _specificationService;
        private readonly IProcessService _processService;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly StepIOInputDTOWrapper _stepIOInputDTOWrapper;
        public ProductService(
        IGenericRepository<Product> productRepository,
        IGenericRepository<ProductionProcessStepIO> stepIORepository,
        IGenericRepository<Material> materialRepository,
        IValidator<ProductInputDTO> productValidator,
        IValidator<ProductDefinitionInputDTO> productDefinitionValidator,
        ICategoryService categoryService,
        ISemiFinishedProductService semiFinishedProductService,
        IMaterialService materialService,
        ISpecificationService specificationService,
        IProcessService processService,
        IMapper mapper,
        EntityListErrorWrapper entityListErrorWrapper,
        StepIOInputDTOWrapper stepIOInputDTOWrapper
        )
        {
            _productRepository = productRepository;
            _stepIORepository = stepIORepository;
            _materialRepository = materialRepository;
            _productValidator = productValidator;
            _productDefinitionValidator = productDefinitionValidator;
            _categoryService = categoryService;
            _semiFinishedProductService = semiFinishedProductService;
            _materialService = materialService;
            _specificationService = specificationService;
            _processService = processService;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
            _stepIOInputDTOWrapper = stepIOInputDTOWrapper;
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
            await _productRepository.Save();
            return new CreateUpdateResponseDTO<Product>
            {
                Id = product.Id,
                Code = product.Code
            };
        }

    }
}