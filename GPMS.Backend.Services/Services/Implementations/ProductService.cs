using System.Net;
using AutoMapper;
using FluentValidation;
using GPMS.Backend.Data.Enums.Statuses.Products;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Products.Specifications;
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
using Microsoft.EntityFrameworkCore;
using GPMS.Backend.Services.PageRequests;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using GPMS.Backend.Data.Models.Staffs;
using GPMS.Backend.Services.Filters;
using AutoMapper.QueryableExtensions;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<QualityStandard> _qualityStandardRepository;
        private readonly IValidator<ProductInputDTO> _productValidator;
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
        private readonly IGenericRepository<Staff> _staffRepository;
        private readonly CurrentLoginUserDTO _currentLoginUser;

        public ProductService(
        IGenericRepository<Product> productRepository,
        IGenericRepository<QualityStandard> qualityStandardRepository,
        IValidator<ProductInputDTO> productValidator,
        ICategoryService categoryService,
        ISemiFinishedProductService semiFinishedProductService,
        IMaterialService materialService,
        ISpecificationService specificationService,
        IProcessService processService,
        IFirebaseStorageService firebaseStorageService,
        IMapper mapper,
        EntityListErrorWrapper entityListErrorWrapper,
        StepIOInputDTOWrapper stepIOInputDTOWrapper,
        QualityStandardImagesTempWrapper qualityStandardImagesTempWrapper,
        IGenericRepository<Staff> staffRepository,
        CurrentLoginUserDTO currentLoginUser
        )
        {
            _productRepository = productRepository;
            _qualityStandardRepository = qualityStandardRepository;
            _productValidator = productValidator;
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
            _staffRepository = staffRepository;
            _currentLoginUser = currentLoginUser;
        }



        public Task AddList(List<ProductInputDTO> inputDTOs, Guid? parentId)
        {
            throw new NotImplementedException();
        }

        #region Get Detail
        public async Task<ProductDTO> Details(Guid id)
        {
            var product = await _productRepository
                .Search(product => product.Id == id)
                .Include(product => product.Category)
                .Include(product => product.SemiFinishedProducts)
                .Include(product => product.Specifications)
                    .ThenInclude(specifications => specifications.Measurements)
                .Include(product => product.Specifications)
                    .ThenInclude(specifications => specifications.BillOfMaterials)
                        .ThenInclude(bom => bom.Material)
                .Include(product => product.Specifications)
                    .ThenInclude(specifications => specifications.QualityStandards)
                .Include(product => product.ProductionProcesses)
                    .ThenInclude(productionProcess => productionProcess.Steps)
                        .ThenInclude(step => step.ProductionProcessStepIOs)
                .Include(product => product.Creator)
                .Include(product => product.Reviewer)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Product not found");
            }

            var productDTO = new ProductDTO
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                Sizes = !string.IsNullOrEmpty(product.Sizes) ? product.Sizes.Split(',').Select(size => size.Trim()).ToList() : new List<string>(),
                Colors = !string.IsNullOrEmpty(product.Colors) ? product.Colors.Split(',').Select(color => color.Trim()).ToList() : new List<string>(),
                // ImageURLs = !string.IsNullOrEmpty(product.ImageURLs) ? product.ImageURLs.Split(';').Select(imageURL => imageURL.Trim()).ToList() : new List<string>(),
                CreatedDate = product.CreatedDate,
                Status = product.Status.ToString(),
                CreatorName = product.Creator.FullName,
                ReviewerName = product.Reviewer?.FullName,
                Category = _mapper.Map<CategoryDTO>(product.Category),
                SemiFinishedProducts = _mapper.Map<List<SemiFinishedProductDTO>>(product.SemiFinishedProducts),

                Specifications = _mapper.Map<List<SpecificationDTO>>(product.Specifications),

                Processes = _mapper.Map<List<ProcessDTO>>(product.ProductionProcesses)
            };
            foreach (ProductSpecification specification in product.Specifications)
            {
                foreach (QualityStandard qualityStandard in specification.QualityStandards)
                {
                    if (!qualityStandard.ImageURL.IsNullOrEmpty())
                    {
                        QualityStandardDTO qualityStandardDTO = productDTO.Specifications
                        .FirstOrDefault(specificationDTO => specificationDTO.Id.Equals(specification.Id))
                        .QualityStandards.FirstOrDefault(qualityStandardDTO => qualityStandardDTO.Id.Equals(qualityStandard.Id));
                        qualityStandardDTO.ImageURL.AddRange(qualityStandard.ImageURL.Split(";", StringSplitOptions.TrimEntries));
                    }
                }
            }
            return productDTO;
        }
        #endregion

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



        #region Add Product
        public async Task<ProductDTO> Add(ProductInputDTO inputDTO)
        {
            ServiceUtils.ValidateInputDTO<ProductInputDTO, Product>
                (inputDTO, _productValidator, _entityListErrorWrapper);
            await ServiceUtils.CheckFieldDuplicatedWithInputDTOAndDatabase<ProductInputDTO, Product>
                (inputDTO, _productRepository, "Code", "Code", _entityListErrorWrapper);
            //Add Category
            CheckCategoryExist(inputDTO.CategoryId);
            //Add Product
            Product product = HandleAddProduct(inputDTO);
            //add semifinish product list
            var semiFinishedProductCodes =
            await _semiFinishedProductService.AddList(inputDTO.SemiFinishedProducts, product.Id);
            //add specification list
            await _specificationService.AddList(inputDTO.Specifications, product.Id, inputDTO.Sizes, inputDTO.Colors);
            //add process list
            var materialIds = inputDTO.Specifications[0].BOMs.Select(bom => bom.MaterialId).ToList();
            await _processService.AddList(inputDTO.Processes, product.Id, materialIds, semiFinishedProductCodes);
            var stepIOWithSemiFinishedProduct = _stepIOInputDTOWrapper.StepIOInputDTOList
                                .Where(stepIO => !stepIO.MaterialId.IsNullOrEmpty())
                                .ToList();
            var stepIOWithMaterialId = _stepIOInputDTOWrapper.StepIOInputDTOList
                                .Where(stepIO => !stepIO.MaterialId.IsNullOrEmpty())
                                .ToList();
            CheckInputDTOsContainsAllSemiFinishProductIds(stepIOWithSemiFinishedProduct, semiFinishedProductCodes);
            CheckInputDTOsContainsAllMaterialId(stepIOWithMaterialId,materialIds);
            if (_entityListErrorWrapper.EntityListErrors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Define Product Failed", _entityListErrorWrapper);
            }
            // await HandleUploadProductImage(inputDTO);
            // await HandleUploadQualityStandardImage();
            await _productRepository.Save();
            var productDTO = _mapper.Map<ProductDTO>(product);
            return productDTO;
        }

        private void CheckCategoryExist(Guid categoryId)
        {
            var existCategory = _categoryService.Details(categoryId);
            if (existCategory == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Category Not Found");
            }
        }
        private Product HandleAddProduct(ProductInputDTO inputDTO)
        {
            Product product = _mapper.Map<Product>(inputDTO);
            product.CategoryId = inputDTO.CategoryId;
            product.CreatorId = _currentLoginUser.Id;
            product.Status = ProductStatus.Pending;
            _productRepository.Add(product);
            return product;
        }

        private void CheckInputDTOsContainsAllSemiFinishProductIds(List<StepIOInputDTO> inputDTOs,
        List<CreateUpdateResponseDTO<SemiFinishedProduct>> semiFinishedProductCodes)
        {
            List<FormError> errors = new List<FormError>();
            foreach (var inputDTO in inputDTOs)
            {
                int equal = 0;
                foreach (var semiCode in semiFinishedProductCodes)
                {
                    if (inputDTO.SemiFinishedProductCode.Equals(semiCode.Code))
                    {
                        equal++;
                    }
                }
                if (equal == 0)
                {
                    errors.Add(new FormError
                    {
                        EntityOrder = inputDTOs.IndexOf(inputDTO) + 1,
                        ErrorMessage = $"SemiFinishedProductCode : {inputDTO.SemiFinishedProductCode} not exist in SemiFinishProduct list of Product",
                        Property = "SemiFinishedProductCode"
                    });
                }
            }
            List<FormError> errorsMissingSemi = new List<FormError>();
            foreach (var semiCode in semiFinishedProductCodes)
            {
                int equal = 0;
                foreach (var inputDTO in inputDTOs)
                {
                    if (semiCode.Code.Equals(inputDTO.SemiFinishedProductCode))
                    {
                        equal++;
                    }
                }
                if (equal == 0)
                {
                    errorsMissingSemi.Add(new FormError
                    {
                        EntityOrder = semiFinishedProductCodes.IndexOf(semiCode) + 1,
                        ErrorMessage = $"StepIO list missing SemiFinishedProductCode : {semiCode.Code}",
                        Property = "Code"
                    });
                }
            }
            if (errors.Count > 0)
            {
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<ProductionProcessStepIO>(errors, _entityListErrorWrapper);
            }
            if (semiFinishedProductCodes.Count > 0)
            {
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<SemiFinishedProduct>(errorsMissingSemi, _entityListErrorWrapper);
            }
        }

        private void CheckInputDTOsContainsAllMaterialId(List<StepIOInputDTO> inputDTOs, List<Guid> materialIds)
        {
            List<FormError> errors = new List<FormError>();
            foreach (var inputDTO in inputDTOs)
            {
                int equal = 0;
                foreach (var materialId in materialIds)
                {
                    if (inputDTO.MaterialId.Equals(materialId))
                    {
                        equal++;
                    }
                }
                if (equal == 0)
                {
                    errors.Add(new FormError
                    {
                        EntityOrder = inputDTOs.IndexOf(inputDTO) + 1,
                        ErrorMessage = $"MaterialId : {inputDTO.SemiFinishedProductCode} not exist in Material list of Specification",
                        Property = "MaterialId"
                    });
                }
            }
            List<FormError> errorsMissingMaterial = new List<FormError>();
            foreach (var materialId in materialIds)
            {
                int equal = 0;
                foreach (var inputDTO in inputDTOs)
                {
                    if (materialId.Equals(inputDTO.MaterialId))
                    {
                        equal++;
                    }
                }
                if (equal == 0)
                {
                    errorsMissingMaterial.Add(new FormError
                    {
                        EntityOrder = materialIds.IndexOf(materialId) + 1,
                        ErrorMessage = $"StepIO list missing MaterialId : {materialId}",
                        Property = "Id"
                    });
                }
            }
            if (errors.Count > 0)
            {
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<ProductionProcessStepIO>(errors, _entityListErrorWrapper);
            }
            if (errorsMissingMaterial.Count > 0)
            {
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<Material>(errorsMissingMaterial, _entityListErrorWrapper);
            }
        }

        // private async Task HandleUploadProductImage(ProductInputDTO inputDTO)
        // {
        //     //upload product image
        //     string fileURL = "";
        //     Product unAddedProduct = _productRepository.GetUnAddedEntity();
        //     foreach (IFormFile file in inputDTO.Definition.Images)
        //     {
        //         if (file != null)
        //         {
        //             string filePath = $"{typeof(Product).Name}/{unAddedProduct.Id}/Images/{file.FileName}";
        //             string url = await _firebaseStorageService.UploadFile(filePath, file);
        //             fileURL += url + ";";
        //         }
        //     }
        //     fileURL = fileURL.Remove(fileURL.Length - 1);
        //     unAddedProduct.ImageURLs = fileURL;
        // }
        // private async Task HandleUploadQualityStandardImage()
        // {
        //     //upload qauality standard image
        //     string fileURL = "";
        //     Product unAddedProduct = _productRepository.GetUnAddedEntity();
        //     List<QualityStandard> unAddedQualityStandardList = _qualityStandardRepository.GetUnAddedEntityList();
        //     foreach (QualityStandardImagesTemp qualityStandardImagesTemp in _qualityStandardImagesTempWrapper.QualityStandardImagesTemps)
        //     {
        //         QualityStandard qualityStandardImageAdd = unAddedQualityStandardList
        //         .Where(qualityStandard => qualityStandard.Id.Equals(qualityStandardImagesTemp.QualityStandardId))
        //         .FirstOrDefault();
        //         if (qualityStandardImageAdd != null)
        //         {
        //             foreach (IFormFile image in qualityStandardImagesTemp.Images)
        //             {
        //                 string filePath =
        //                 $"{typeof(Product).Name}/{unAddedProduct.Id}/{typeof(ProductSpecification).Name}/" +
        //                 $"{qualityStandardImageAdd.ProductSpecificationId}/{typeof(QualityStandard).Name}/" +
        //                 $"{qualityStandardImageAdd.Id}/Images/{image.FileName}";
        //                 string url = await _firebaseStorageService.UploadFile(filePath, image);
        //                 fileURL += url + ";";
        //             }
        //             fileURL = fileURL.Remove(fileURL.Length - 1);
        //             qualityStandardImageAdd.ImageURL = fileURL;
        //             fileURL = "";
        //         }
        //     }
        // }

        #endregion Add Product

        #region Change Product Status

        public async Task<ProductDTO> ChangeStatus(Guid id, string productStatus)
        {
            var product = _productRepository.Details(id);

            if (product == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Product not found");
            }
            ProductStatus parsedStatus = ValidateProductStatus(productStatus, product);
            if (parsedStatus.Equals(ProductStatus.Approved) && product.Status.Equals(ProductStatus.Pending))
                product.ReviewerId = _currentLoginUser.Id;
            product.Status = parsedStatus;
            await _productRepository.Save();
            return _mapper.Map<ProductDTO>(product);
        }

        private ProductStatus ValidateProductStatus(string productStatus, Product product)
        {
            if (!Enum.TryParse(productStatus, true, out ProductStatus parsedStatus))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Invalid status value provided.");
            }
            if (product.Status.Equals(ProductStatus.Pending) && parsedStatus.Equals(ProductStatus.InProduction))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Can not change status of product from Pending to InProduction");
            }
            if (product.Status.Equals(ProductStatus.Approved))
            {
                if (parsedStatus.Equals(ProductStatus.Declined))
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Can not change status of product from Approved to Declined");
                }
                if (parsedStatus.Equals(ProductStatus.Pending))
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Can not change status of product from Approved to Pending");
                }
            }
            if (product.Status.Equals(ProductStatus.InProduction))
            {
                if (parsedStatus.Equals(ProductStatus.Declined))
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Can not change status of product from InProduction to Declined");
                }
                if (parsedStatus.Equals(ProductStatus.Pending))
                {
                    throw new APIException((int)HttpStatusCode.BadRequest, "Can not change status of product from InProduction to Pending");
                }
            }
            if (product.Status.Equals(ProductStatus.Declined))
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Can not change status of product is already Declined");
            }
            return parsedStatus;
        }

        #endregion Change Product Status

        #region Get All Product

        public async Task<DefaultPageResponseListingDTO<ProductListingDTO>> GetAll(ProductFilterModel productFilterModel)
        {
            IQueryable<Product> query = _productRepository.GetAll();
            query = Filter(query, productFilterModel);
            query = query.SortBy(productFilterModel);
            List<Product> productList = await query.ToListAsync();
            productList = FilterColorAndSize(productList, productFilterModel);
            int totalItem = productList.Count;
            productList = productList.PagingEntityList(productFilterModel);
            List<ProductListingDTO> productListingDTOs = new List<ProductListingDTO>();
            foreach (Product product in productList)
            {
                ProductListingDTO productListingDTO = _mapper.Map<ProductListingDTO>(product);
                if (!product.Sizes.IsNullOrEmpty())
                {
                    string[] sizeArr = product.Sizes.Split(",", StringSplitOptions.TrimEntries);
                    productListingDTO.Sizes.AddRange(sizeArr);
                }
                else productListingDTO.Sizes = null;
                if (!product.Colors.IsNullOrEmpty())
                {
                    string[] colorArr = product.Colors.Split(",", StringSplitOptions.TrimEntries);
                    productListingDTO.Colors.AddRange(colorArr);
                }
                else productListingDTO.Colors = null;
                productListingDTOs.Add(productListingDTO);
            }

            return new DefaultPageResponseListingDTO<ProductListingDTO>
            {
                Data = productListingDTOs,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = productFilterModel.Pagination.PageIndex,
                    PageSize = productFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }

        private List<Product> FilterColorAndSize(List<Product> productList, ProductFilterModel productFilterModel)
        {
            if (!productFilterModel.Size.IsNullOrEmpty())
            {
                productList = productList
                .Where(product => product.Sizes.Split(",", StringSplitOptions.TrimEntries)
                                                .Select(size => size.ToLower())
                                                .Contains(productFilterModel.Size.ToLower()))
                                                .ToList();
            }
            if (!productFilterModel.Color.IsNullOrEmpty())
            {
                productList = productList
                .Where(product => product.Colors.Split(",", StringSplitOptions.TrimEntries)
                                                .Select(color => color.ToLower())
                                                .Contains(productFilterModel.Color.ToLower()))
                                                .ToList();
            }
            return productList;
        }

        private IQueryable<Product> Filter(IQueryable<Product> query, ProductFilterModel productFilterModel)
        {
            if (!productFilterModel.Code.IsNullOrEmpty())
            {
                query = query
                .Where(product => product.Code.ToLower().Contains(productFilterModel.Code.ToLower()));
            }

            if (!productFilterModel.Name.IsNullOrEmpty())
            {
                query = query
                .Where(product => product.Name.ToLower().Contains(productFilterModel.Name.ToLower()));
            }

            if (!productFilterModel.Status.IsNullOrEmpty()
                    && Enum.TryParse<ProductStatus>(productFilterModel.Status, true, out ProductStatus parsedProductStatus))
            {
                query = query
                .Where(product => product.Status.Equals(parsedProductStatus));
            }
            return query;
        }

        public async Task<List<CreateProductListingDTO>> GetAllProductForCreateProductionPlan()
        {
            List<Product> products = await _productRepository.GetAll().ToListAsync();

            return _mapper.Map<List<CreateProductListingDTO>>(products);

        }
        #endregion Get All Product

    }
}