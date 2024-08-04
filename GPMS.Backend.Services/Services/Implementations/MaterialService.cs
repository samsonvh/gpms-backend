using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using FluentValidation.Results;
using GPMS.Backend.Data.Models.Products;
using GPMS.Backend.Data.Repositories;
using GPMS.Backend.Services.DTOs;
using GPMS.Backend.Services.DTOs.InputDTOs.Product;
using GPMS.Backend.Services.DTOs.LisingDTOs;
using GPMS.Backend.Services.DTOs.ResponseDTOs;
using GPMS.Backend.Services.Exceptions;
using GPMS.Backend.Services.Filters;
using GPMS.Backend.Services.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Services.Implementations
{
    public class MaterialService : IMaterialService
    {
        private readonly IGenericRepository<Material> _materialRepository;
        private readonly IValidator<MaterialInputDTO> _materialValidator;
        private readonly IMapper _mapper;
        private readonly EntityListErrorWrapper _entityListErrorWrapper;

        public MaterialService(
            IGenericRepository<Material> materialRepository,
            IValidator<MaterialInputDTO> materialValidator,
            IMapper mapper,
            EntityListErrorWrapper entityListErrorWrapper)
        {
            _materialRepository = materialRepository;
            _materialValidator = materialValidator;
            _mapper = mapper;
            _entityListErrorWrapper = entityListErrorWrapper;
        }


        public async Task AddList(List<MaterialInputDTO> inputDTOs, Guid? parentEntityId = null)
        {
            throw new NotImplementedException();
        }
        #region Add List
        public async Task<List<CreateUpdateResponseDTO<Material>>> AddList(List<MaterialInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }
        #endregion
        public async Task<MaterialDTO> Details(Guid id)
        {
            var material = await _materialRepository
                .Search(material => material.Id == id)
                .FirstOrDefaultAsync();
            if (material == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Material not found");
            }
            return _mapper.Map<MaterialDTO>(material);
        }

        public async Task<DefaultPageResponseListingDTO<MaterialListingDTO>> GetAll(MaterialFilterModel materialFilterModel)
        {
            var query = _materialRepository.GetAll();
            query = Filters(query, materialFilterModel);
            query = query.SortBy<Material>(materialFilterModel);
            int totalItem = query.Count();
            query = query.PagingEntityQuery(materialFilterModel);
            var data = await query.ProjectTo<MaterialListingDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return new DefaultPageResponseListingDTO<MaterialListingDTO>
            {
                Data = data,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = materialFilterModel.Pagination.PageIndex,
                    PageSize = materialFilterModel.Pagination.PageSize,
                    TotalRows = totalItem
                }
            };

        }

        private IQueryable<Material> Filters(IQueryable<Material> query, MaterialFilterModel materialFilterModel)
        {
            if (!materialFilterModel.Code.IsNullOrEmpty())
            {
                query = query.Where(material => material.Code.Contains(materialFilterModel.Code));
            }
            if (!materialFilterModel.Name.IsNullOrEmpty())
            {
                query = query.Where(material => material.Name.Contains(materialFilterModel.Name));
            }
            return query;
        }


        public Task UpdateList(List<MaterialInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }

        public Task<DefaultPageResponseListingDTO<MaterialListingDTO>> GetAll(int productId, MaterialFilterModel filter)
        {
            throw new NotImplementedException();
        }
        #region Add 
        public async Task<MaterialDTO> Add(MaterialInputDTO inputDTO)
        {
            ServiceUtils.ValidateInputDTO<MaterialInputDTO, Material>
                (inputDTO, _materialValidator, _entityListErrorWrapper);
            List<MaterialInputDTO> inputDTOs = [inputDTO];
            await ServiceUtils.CheckFieldDuplicatedWithInputDTOAndDatabase<MaterialInputDTO,Material>
                (inputDTO,_materialRepository,"Code","Code",_entityListErrorWrapper);
            if (_entityListErrorWrapper.EntityListErrors.Count > 0)
            {
                throw new APIException((int)HttpStatusCode.BadRequest, "Create Material Failed", _entityListErrorWrapper);
            }
            Material material = _mapper.Map<Material>(inputDTO);
            _materialRepository.Add(material);
            await _materialRepository.Save();
            return _mapper.Map<MaterialDTO>(material);
        }
        #endregion
        #region Update 
        public async Task<MaterialDTO> Update(Guid id, MaterialInputDTO inputDTO)
        {
            var existedMaterial = _materialRepository.Details(id);
            if (existedMaterial == null)
            {
                throw new APIException((int)HttpStatusCode.NotFound, "Material Not Found");
            }
            ServiceUtils.ValidateInputDTO<MaterialInputDTO, Material>
                (inputDTO, _materialValidator, _entityListErrorWrapper);
            existedMaterial.Code = inputDTO.Code;
            existedMaterial.Name = inputDTO.Name;
            existedMaterial.ConsumptionUnit = inputDTO.ConsumptionUnit;
            existedMaterial.SizeWidthUnit = inputDTO.SizeWidthUnit;
            existedMaterial.ColorCode = inputDTO.ColorCode;
            existedMaterial.ColorName = inputDTO.ColorName;
            existedMaterial.Description = inputDTO.Description;
            _materialRepository.Update(existedMaterial);
            await _materialRepository.Save();
            return _mapper.Map<MaterialDTO>(existedMaterial);
        }
        #endregion
    }
}