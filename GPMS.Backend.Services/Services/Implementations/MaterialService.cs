using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
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

        public Task<CreateUpdateResponseDTO<Material>> Add(MaterialInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public async Task AddList(List<MaterialInputDTO> inputDTOs, Guid? parentEntityId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CreateUpdateResponseDTO<Material>>> AddList(List<MaterialInputDTO> inputDTOs)
        {
            ServiceUtils.ValidateInputDTOList<MaterialInputDTO, Material>
                (inputDTOs, _materialValidator,_entityListErrorWrapper);
            ServiceUtils.CheckFieldDuplicatedInInputDTOList<MaterialInputDTO, Material>
                (inputDTOs, "Code",_entityListErrorWrapper);
            await CheckMaterialCodeInMaterialInInputDTOList(inputDTOs);
            List<CreateUpdateResponseDTO<Material>> responses = new List<CreateUpdateResponseDTO<Material>>();
            foreach (MaterialInputDTO materialInputDTO in inputDTOs)
            {
                Material material = _mapper.Map<Material>(materialInputDTO);
                CreateUpdateResponseDTO<Material> response = new CreateUpdateResponseDTO<Material>
                {
                    Code = materialInputDTO.Code
                };
                if (materialInputDTO.IsNew)
                {
                    _materialRepository.Add(material);
                    response.Id = material.Id;
                }
                else
                {
                    Material existedMaterial = await _materialRepository
                    .Search(material => material.Code.Equals(materialInputDTO.Code))
                    .FirstOrDefaultAsync();
                    response.Id = existedMaterial.Id;
                }

                responses.Add(response);
            }
            return responses;
        }

        private async Task CheckMaterialCodeInMaterialInInputDTOList(List<MaterialInputDTO> inputDTOs)
        {
            List<FormError> errors = new List<FormError>();
            foreach (MaterialInputDTO materialInputDTO in inputDTOs)
            {
                int entityOrder = 1;
                Material existedMaterial = await _materialRepository
                                            .Search(material => material.Code.Equals(materialInputDTO.Code))
                                            .FirstOrDefaultAsync();
                if (materialInputDTO.IsNew && existedMaterial != null)
                {
                    errors.Add(new FormError
                    {
                        Property = "Code",
                        ErrorMessage = $"There is a {typeof(Material).Name} with Code : {materialInputDTO.Code} duplicated in system",
                        EntityOrder = entityOrder
                    });
                }
                else if (!materialInputDTO.IsNew && existedMaterial == null)
                {
                    errors.Add(new FormError
                    {
                        Property = materialInputDTO.GetType().GetProperty("Code").Name,
                        ErrorMessage = $"There is a {typeof(Material).Name} with Code : {materialInputDTO.Code} is not existed in system",
                        EntityOrder = entityOrder
                    });
                }
                entityOrder++;
            }
            if (errors.Count > 0)
            {
                ServiceUtils.CheckErrorWithEntityExistAndAddErrorList<Material>(errors,_entityListErrorWrapper);
            }
        }

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
            var data = _mapper.Map<List<MaterialListingDTO>>(await _materialRepository.GetAll().ToListAsync());
            return new DefaultPageResponseListingDTO<MaterialListingDTO>
            {
                Data = data,
                Pagination = new PaginationResponseModel
                {
                    PageIndex = materialFilterModel.PageIndex,
                    PageSize = materialFilterModel.PageSize,
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

        public Task<CreateUpdateResponseDTO<Material>> Update(MaterialInputDTO inputDTO)
        {
            throw new NotImplementedException();
        }

        public Task UpdateList(List<MaterialInputDTO> inputDTOs)
        {
            throw new NotImplementedException();
        }

    }
}