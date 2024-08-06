using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Models.Products.ProductionProcesses;
using GPMS.Backend.Data.Models.Products.Specifications;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product.Specification;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.PageRequests;
using GPMS.Backend.Services.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class QualityStandardService : IQualityStandardService
    {
        private readonly IGenericRepository<QualityStandard> _qualityStandardRepository;
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IValidator<QualityStandardInputDTO> _qualityStandardValidator;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;
        private readonly QualityStandardImagesTempWrapper _qualityStandardImagesTempWrapper;
        public QualityStandardService(
            IGenericRepository<QualityStandard> qualityStandardRepository,
            IFirebaseStorageService firebaseStorageService,
            IValidator<QualityStandardInputDTO> qualityStandardValidator,
            IMapper mapper,
            EntityListErrorWrapper entityListErrorWrapper,
            QualityStandardImagesTempWrapper qualityStandardImagesTempWrapper
            )
        {
            _qualityStandardRepository = qualityStandardRepository;
            _firebaseStorageService = firebaseStorageService;
            _qualityStandardValidator = qualityStandardValidator;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
            _qualityStandardImagesTempWrapper = qualityStandardImagesTempWrapper;
        }


        public Task AddList(List<QualityStandardInputDTO> inputDTOs, Guid? parentEntityId = null)
        {
            throw new NotImplementedException();
        }

        #region Add Quality Standard
        public async Task AddList(List<QualityStandardInputDTO> inputDTOs, Guid specificationId, List<Guid> materialIds)
        {
            ServiceUtils.ValidateInputDTOList<QualityStandardInputDTO, QualityStandard>
                (inputDTOs, _qualityStandardValidator, _entityListErrorWrapper);
            CheckMaterialIdExistInMaterialIds(inputDTOs, materialIds);
            foreach (QualityStandardInputDTO qualityStandardInputDTO in inputDTOs)
            {
                QualityStandard qualityStandard = _mapper.Map<QualityStandard>(qualityStandardInputDTO);
                qualityStandard.ProductSpecificationId = specificationId;
                _qualityStandardRepository.Add(qualityStandard);
            }
        }

        private void CheckMaterialIdExistInMaterialIds(List<QualityStandardInputDTO> inputDTOs, List<Guid> materialIds)
        {
            List<FormError> errors = new List<FormError>();
            foreach (var inputDTO in inputDTOs)
            {
                if (!materialIds.Contains(inputDTO.MaterialId))
                {
                    errors.Add
                    (
                        new FormError
                        {
                            EntityOrder = inputDTOs.IndexOf(inputDTO) + 1,
                            ErrorMessage = $"MaterialId : {inputDTO.MaterialId} of Quality Standard not exist in material list in BOM",
                            Property = "MaterialId"
                        }

                    );
                }
            }
            if (errors.Count > 0)
            {
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<QualityStandard>(errors, _entityListErrorWrapper);
            }
        }


        #endregion
        public Task<QualityStandardDTO> Details(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponseListingDTO<QualityStandardListingDTO>> GetAll(QualityStandardFilterModel qualityStandardFilterModel)
        {
            var query = _qualityStandardRepository.GetAll();
            query = Filters(query, qualityStandardFilterModel);
            query = query.SortBy<QualityStandard>(qualityStandardFilterModel);
            List<QualityStandard> qualityStandardList = await query.ToListAsync();
            int totalItem = query.Count();
            query = query.PagingEntityQuery(qualityStandardFilterModel);
            var data = await query.ProjectTo<QualityStandardListingDTO>(_mapper.ConfigurationProvider).ToListAsync();

            List<QualityStandardListingDTO> qualityListingDTOs = new List<QualityStandardListingDTO>();
            foreach (QualityStandard qualityStandard in qualityStandardList)
            {
                QualityStandardListingDTO qualityListingDTO = _mapper.Map<QualityStandardListingDTO>(qualityStandard);
                if (!qualityStandard.ImageURL.IsNullOrEmpty())
                {
                    string[] imageArr = qualityStandard.ImageURL.Split(";", StringSplitOptions.None);
                    qualityListingDTO.ImageURL.AddRange(imageArr);
                }
                qualityListingDTOs.Add(qualityListingDTO);
            }

            return new DefaultPageResponseListingDTO<QualityStandardListingDTO>
            {
                Data = qualityListingDTOs,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = qualityStandardFilterModel.Pagination.PageIndex,
                    PageSize = qualityStandardFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }

        private IQueryable<QualityStandard> Filters(IQueryable<QualityStandard> query, QualityStandardFilterModel qualityStandardFilterModel)
        {
            if (!qualityStandardFilterModel.Name.IsNullOrEmpty())
            {
                query = query.Where(process => process.Name.Contains(qualityStandardFilterModel.Name));
            }
            return query;
        }


        public Task UpdateList(List<QualityStandardInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponseListingDTO<QualityStandardListingDTO>> GetAllQualityOfSpecification(Guid specificationId, QualityStandardFilterModel qualityStandardFilterModel)
        {
            var query = _qualityStandardRepository.GetAll();
            query = Filters(query, qualityStandardFilterModel);
            query = query.Where(qualityStandard => qualityStandard.ProductSpecificationId == specificationId);
            query = query.SortBy<QualityStandard>(qualityStandardFilterModel);
            List<QualityStandard> qualityStandardList = await query.ToListAsync();
            int totalItem = query.Count();
            query = query.PagingEntityQuery(qualityStandardFilterModel);
            var data = await query.ProjectTo<QualityStandardListingDTO>(_mapper.ConfigurationProvider).ToListAsync();

            List<QualityStandardListingDTO> qualityListingDTOs = new List<QualityStandardListingDTO>();
            foreach (QualityStandard qualityStandard in qualityStandardList)
            {
                QualityStandardListingDTO qualityListingDTO = _mapper.Map<QualityStandardListingDTO>(qualityStandard);
                if (!qualityStandard.ImageURL.IsNullOrEmpty())
                {
                    string[] imageArr = qualityStandard.ImageURL.Split(";", StringSplitOptions.None);
                    qualityListingDTO.ImageURL.AddRange(imageArr);
                    var firstImageURL = imageArr.FirstOrDefault();

                }
                qualityListingDTOs.Add(qualityListingDTO);
            }

            return new DefaultPageResponseListingDTO<QualityStandardListingDTO>
            {
                Data = qualityListingDTOs,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = qualityStandardFilterModel.Pagination.PageIndex,
                    PageSize = qualityStandardFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };
        }

        public Task<QualityStandardDTO> Add(QualityStandardInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task<QualityStandardDTO> Update(Guid id, QualityStandardInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        #region Upload Image
        public async Task<QualityStandardDTO> UploadImages(ImageQualityStandardInputDTO inputDTO)
        {
            var qualityStandardExist = await _qualityStandardRepository
                .Search(qualityStandard => qualityStandard.Id.Equals(inputDTO.QualityStandardId))
                .Include(qualityStandard => qualityStandard.ProductSpecification)
                    .ThenInclude(specification => specification.Product)
                    .FirstOrDefaultAsync();
            if (qualityStandardExist == null)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Quality Standard Not Found");
            }
            var updatedQualityStandard = await HandleUploadSpecificationImage(inputDTO,qualityStandardExist);
            return _mapper.Map<QualityStandardDTO>(updatedQualityStandard);
        }

        private async Task<QualityStandard> HandleUploadSpecificationImage
            (ImageQualityStandardInputDTO inputDTO, QualityStandard qualityStandard)
        {
            //upload qualityStandard image
            string fileURL = "";
            foreach (IFormFile file in inputDTO.Images)
            {
                if (file != null)
                {
                    string filePath = $"{typeof(Product).Name}/{qualityStandard.ProductSpecification.ProductId}/{typeof(ProductSpecification).Name}/{qualityStandard.ProductSpecificationId}/{typeof(QualityStandard).Name}/{qualityStandard.Id}/Images/{file.FileName}";
                    string url = await _firebaseStorageService.UploadFile(filePath, file);
                    fileURL += url + ";";
                }
            }
            fileURL = fileURL.Remove(fileURL.Length - 1);
            qualityStandard.ImageURL = fileURL;
            _qualityStandardRepository.Update(qualityStandard);
            await _qualityStandardRepository.Save();
            return qualityStandard;
        }

        #endregion
    }
}